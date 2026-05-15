using System.IO.Pipelines;
using System.Threading.Channels;

using Microsoft.Extensions.Logging;

using Serilog.Context;

using Sharpmine.Server.Core.Protocol.Extensions;
using Sharpmine.Server.Core.Protocol.Packets;
using Sharpmine.Server.Core.Protocol.Packets.Abstract.Clientbound;
using Sharpmine.Server.Core.Protocol.Packets.Abstract.Serverbound;
using Sharpmine.Server.Core.Protocol.Packets.Login.Clientbound;
using Sharpmine.Server.Core.Security;

namespace Sharpmine.Server.Core.Protocol;

public sealed partial class ClientHandler(
    string ip,
    TcpClient client,
    PacketReceiver packetReceiver,
    PacketDispatcher packetDispatcher,
    PacketTransmitter packetTransmitter,
    ServerCapacityManager serverCapacityManager,
    ILogger<ClientHandler> logger) : IDisposable
{

    private readonly Channel<IClientboundPacket> _clientboundChannel = Channel.CreateClientbound();

    private readonly Channel<IServerboundPacket> _serverboundChannel = Channel.CreateServerbound();

    private CancellationTokenSource? _cts;

    private Task? _transmissionTask;

    private volatile bool _disconnecting;

    private volatile bool _aborted;

    public event Action? Terminated;

    public Guid Id { get; } = Guid.CreateVersion7();

    public string Ip { get; } = ip;

    public TcpClient Client { get; } = client;

    public ProtocolState State { get; private set; } = ProtocolState.Handshake;

    public bool OccupiesPlayerSlot { get; internal set; }

    public ClientInformationPacket? Information { get; internal set; }

    public async Task HandleAsync(CancellationToken cancellationToken)
    {
        await using var stream = Client.GetStream();
        var writer = PipeWriter.Create(stream);
        var reader = PipeReader.Create(stream);
        using var _ = LogContext.PushProperty("ClientHandlerId", Id);
        LogClientConnected(this);
        Task? dispatchTask = null;

        try
        {
            if (_aborted)
            {
                return;
            }

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _transmissionTask = new TransmissionWorker(_clientboundChannel, this, writer, packetTransmitter).StartAsync(_cts.Token);
            dispatchTask = new DispatchWorker(_serverboundChannel, this, packetDispatcher).StartAsync(_cts.Token);

            while (await TryReceivePacketAsync(reader, _cts.Token)) ;
        }
        catch (OperationCanceledException)
        {
            LogDisconnectingClient(this, "Operation canceled");
        }
        catch (Exception ex) when (ex is SocketException or IOException)
        {
            LogClientDisconnected(this);
        }
        catch (Exception ex)
        {
            LogErrorWhileHandling(ex);
            await DisconnectAsync("Internal server error");
        }
        finally
        {
            await AbortForcefullyAsync();

            try
            {
                if (_transmissionTask is not null && dispatchTask is not null) await Task.WhenAll(_transmissionTask, dispatchTask);
                else if (_transmissionTask is not null) await _transmissionTask;
                else if (dispatchTask is not null) await dispatchTask;
            }
            catch
            {
                // It is safe to ignore everything here
            }

            await reader.CompleteAsync();
            await writer.CompleteAsync();
        }
    }

    private async Task<bool> TryReceivePacketAsync(PipeReader pipeReader, CancellationToken cancellationToken)
    {
        var (keepAlive, packet) = await packetReceiver.ReceiveAsync(State, pipeReader, cancellationToken);

        if (!keepAlive)
        {
            return false;
        }

        if (packet is IStateTransition transition)
        {
            LogStateTransition(State, transition.NextState);
            State = transition.NextState;
        }

        if (packet is not null && !_serverboundChannel.Writer.TryWrite(packet) && !_disconnecting)
        {
            LogDisconnectingClient(this, "Too many serverbound packets queued");
            return false;
        }

        return true;
    }

    public void SendPacket(IClientboundPacket packet)
    {
        if (packet.State != State)
        {
            LogUnmatchedStates(packet, State);
            AbortForcefully();
            return;
        }

        // Checking _disconnecting **after** attempting to write to the channel to prevent a race condition
        // that would cause forceful abortion even though graceful disconnection might already be in progress
        if (!_clientboundChannel.Writer.TryWrite(packet) && !_disconnecting)
        {
            LogDisconnectingClient(this, "Too many clientbound packets queued");
            AbortForcefully();
        }
    }

    public Task DisconnectAsync(string reason)
    {
        if (State == ProtocolState.Login)
        {
            return DisconnectAsync(new LoginDisconnectPacket(reason));
        }

        try
        {
            return DisconnectAsync(DisconnectPacket.Create(State) with { Reason = reason });
        }
        catch (ArgumentOutOfRangeException ex)
        {
            LogDisconnectRequestedInInvalidState(ex, State);
            return AbortForcefullyAsync();
        }
    }

    public async Task DisconnectAsync(IClientboundPacket disconnectPacket)
    {
        if (Interlocked.Exchange(ref _disconnecting, true))
        {
            return;
        }

        _clientboundChannel.Writer.TryWrite(disconnectPacket);
        await AbortGracefullyAsync();
    }

    public async Task AbortGracefullyAsync()
    {
        TryCompleteChannelWriters();

        if (_transmissionTask is null)
        {
            await AbortForcefullyAsync();
            return;
        }

        try
        {
            await _transmissionTask.WaitAsync(TimeSpan.FromSeconds(2));
        }
        catch (TimeoutException)
        {
            // Force kill
        }

        await AbortForcefullyAsync();
    }

    public Task AbortForcefullyAsync()
    {
        _aborted = true;
        _disconnecting = true;
        TryCompleteChannelWriters();

        try
        {
            return _cts?.CancelAsync() ?? Task.CompletedTask;
        }
        catch (ObjectDisposedException)
        {
            return Task.CompletedTask;
        }
    }

    public void AbortForcefully()
    {
        _aborted = true;
        _disconnecting = true;
        TryCompleteChannelWriters();

        try
        {
            _cts?.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // Do nothing, _cts already canceled
        }
    }

    private void TryCompleteChannelWriters()
    {
        _clientboundChannel.Writer.TryComplete();
        _serverboundChannel.Writer.TryComplete();
    }

    public void Dispose()
    {
        Client.Dispose();
        _cts?.Dispose();
        _cts = null;

        if (OccupiesPlayerSlot)
        {
            serverCapacityManager.ReleaseSlot();
            OccupiesPlayerSlot = false;
        }

        Terminated?.Invoke();
    }

    public override string ToString() => Ip;

}
