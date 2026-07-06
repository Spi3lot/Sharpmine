using System.IO.Pipelines;
using System.Threading.Channels;

using Microsoft.Extensions.Logging;

using Serilog.Context;

using Sharpmine.Domain.DataTypes.Components;
using Sharpmine.Server.Domain.Entities;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Packets;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Clientbound;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Serverbound;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Login.Clientbound;
using Sharpmine.Server.Infrastructure.Security;

using KeepAlivePacket = Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Clientbound.KeepAlivePacket;

namespace Sharpmine.Server.Infrastructure.Protocol;

public sealed partial class ClientHandler(
    string ip,
    TcpClient client,
    PacketReceiver packetReceiver,
    DispatchWorkerFactory dispatchWorkerFactory,
    TransmissionWorkerFactory transmissionWorkerFactory,
    ServerCapacityManager serverCapacityManager,
    ILogger<ClientHandler> logger) : IDisposable
{

    private readonly Channel<IClientboundPacket> _clientboundChannel = Channel.CreateClientbound();

    private readonly Channel<IServerboundPacket> _serverboundChannel = Channel.CreateServerbound();

    private CancellationTokenSource? _cts;

    private Task? _transmissionTask;

    private volatile bool _disconnecting;

    private volatile bool _aborted;

    public event EventHandler? Terminated;

    public Guid Id { get; } = Guid.CreateVersion7();

    public string Ip { get; } = ip;

    public TcpClient Client { get; } = client;

    public ProtocolState State { get; private set; } = ProtocolState.Handshake;

    public ClientInformationPacket? Information { get; internal set; }

    public Player? Player { get; internal set; }

    public long CurrentKeepAliveId { get; private set; }

    public bool WaitingForKeepAlive { get; set; }

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
            _transmissionTask = transmissionWorkerFactory.Create(_clientboundChannel, this, writer).StartAsync(CancellationToken.None);
            dispatchTask = dispatchWorkerFactory.Create(_serverboundChannel, this).StartAsync(_cts.Token);
            StartKeepAliveLoopAsync(_cts.Token);

            while (await TryReceivePacketAsync(reader, _cts.Token)) ;
        }
        catch (OperationCanceledException)
        {
            LogDisconnectingClient(this, "Server closed");

            await DisconnectAsync(new TextComponent("Server closed")
            {
                Style = new ComponentStyle { Color = Color.Gold.Name }
            });
        }
        catch (Exception ex) when (ex is SocketException or ObjectDisposedException or { InnerException: SocketException or ObjectDisposedException or { InnerException: SocketException or ObjectDisposedException } })
        {
            LogClientDisconnected(this);
        }
        catch (Exception ex)
        {
            LogErrorWhileHandling(ex);

            await DisconnectAsync(new TextComponent("Internal server error")
            {
                Style = new ComponentStyle { Color = Color.DarkRed.Name }
            });
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
        if (!keepAlive) return false;
        if (packet is null) return true;

        if (packet is IStateTransition transition)
        {
            LogStateTransition(State, transition.NextState);
            State = transition.NextState;
        }

        if (await _serverboundChannel.Writer.WaitToWriteAsync(_cts!.Token))
        {
            if (!_serverboundChannel.Writer.TryWrite(packet) && !_disconnecting)
            {
                LogDisconnectingClient(this, "Channel write failed");
                return false;
            }
        }
        else if (!_disconnecting)
        {
            LogDisconnectingClient(this, "Channel no longer accepting packets");
            return false;
        }

        return true;
    }

    private async Task StartKeepAliveLoopAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

        try
        {
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                if (WaitingForKeepAlive)
                {
                    await DisconnectAsync(new TextComponent("Timed out")
                    {
                        Style = new ComponentStyle { Color = Color.Red.Name }
                    });

                    break;
                }

                if (KeepAlivePacket.Exists(State))
                {
                    WaitingForKeepAlive = true;
                    CurrentKeepAliveId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    SendPacket(KeepAlivePacket.Create(State) with { KeepAliveId = CurrentKeepAliveId });
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Connection closed, timer canceled
        }
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

    public Task DisconnectAsync(Component reason)
    {
        if (State == ProtocolState.Login)
        {
            return DisconnectAsync(new LoginDisconnectPacket(reason));
        }

        if (DisconnectPacket.Exists(State))
        {
            return DisconnectAsync(DisconnectPacket.Create(State) with { Reason = reason });
        }

        LogDisconnectRequestedInInvalidState(State);
        return AbortForcefullyAsync();
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
        serverCapacityManager.TryReleaseSlot(Id);
        Terminated?.Invoke(this, EventArgs.Empty);
    }

    public override string ToString() => Ip;

}
