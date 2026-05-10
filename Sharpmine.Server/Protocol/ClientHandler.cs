using System.Text;
using System.Threading.Channels;

using Microsoft.Extensions.Logging;

using Serilog.Context;

using Sharpmine.Server.Protocol.Extensions;
using Sharpmine.Server.Protocol.Packets;
using Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;
using Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;
using Sharpmine.Server.Security;

namespace Sharpmine.Server.Protocol;

public sealed partial class ClientHandler(
    string ip,
    TcpClient client,
    ServerService server,
    PacketTransceiver packetTransceiver,
    PacketDispatcher packetDispatcher,
    ServerCapacityManager serverCapacityManager,
    ILogger<ClientHandler> logger)
{

    private readonly Channel<IClientboundPacket> _clientboundChannel = Channel.CreateClientbound();

    private readonly Channel<IServerboundPacket> _serverboundChannel = Channel.CreateServerbound();

    private CancellationTokenSource? _cts;

    private Task? _transmitTask;

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
        await using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);
        using var _ = LogContext.PushProperty("ClientHandlerId", Id);
        LogClientConnected(this);
        Task? processTask = null;

        try
        {
            if (_aborted)
            {
                return;
            }

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _transmitTask = new ClientboundChannelWorker(_clientboundChannel, this, stream, writer, packetTransceiver).StartAsync(_cts.Token);
            processTask = new ServerboundChannelWorker(_serverboundChannel, this, packetDispatcher).StartAsync(_cts.Token);

            while (await TryReceivePacketAsync(stream, reader, _cts.Token)) ;
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
            await AbortAsync();

            try
            {
                if (_transmitTask is not null && processTask is not null) await Task.WhenAll(_transmitTask, processTask);
                else if (_transmitTask is not null) await _transmitTask;
                else if (processTask is not null) await processTask;
            }
            catch
            {
                // It is safe to ignore everything here
            }

            Cleanup();
        }
    }

    private async Task<bool> TryReceivePacketAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        var (keepAlive, packet) = await packetTransceiver.ReceiveAsync(State, stream, reader, cancellationToken);

        if (!keepAlive)
        {
            return false;
        }

        if (packet is IStateTransition transition)
        {
            LogStateTransition(State, transition.NextState);
            State = transition.NextState;
        }

        if (packet is not null && !_serverboundChannel.Writer.TryWrite(packet))
        {
            LogDisconnectingClient(this, "Too many serverbound packets queued");
            return false;
        }

        return true;
    }

    public void SendPacket(IClientboundPacket packet)
    {
        if (_disconnecting)
        {
            return;
        }

        if (packet.State != State)
        {
            LogUnmatchedStates(packet, State);
            Abort();
            return;
        }

        if (!_clientboundChannel.Writer.TryWrite(packet))
        {
            LogDisconnectingClient(this, "Too many clientbound packets queued");
            Abort();
        }
    }

    public async Task DisconnectAsync(string reason)
    {
        if (Interlocked.Exchange(ref _disconnecting, true))
        {
            return;
        }

        if (DisconnectPacket.Create(State) is not { } packet)
        {
            await AbortAsync();
            return;
        }

        _clientboundChannel.Writer.TryWrite(packet with { Reason = reason });
        _clientboundChannel.Writer.Complete();

        if (_transmitTask is null)
        {
            await AbortAsync();
            return;
        }

        try
        {
            await _transmitTask.WaitAsync(TimeSpan.FromSeconds(2));
        }
        catch (TimeoutException)
        {
            // Force kill
        }

        await AbortAsync();
    }

    public Task AbortAsync()
    {
        _aborted = true;
        _disconnecting = true;

        try
        {
            return _cts?.CancelAsync() ?? Task.CompletedTask;
        }
        catch (ObjectDisposedException)
        {
            return Task.CompletedTask;
        }
    }

    public void Abort()
    {
        _aborted = true;
        _disconnecting = true;

        try
        {
            _cts?.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // Do nothing, _cts already canceled
        }
    }

    private void Cleanup()
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
