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
    PacketDispatcher dispatcher,
    PlayerAccessManager playerAccessManager, // TODO: Use
    ILogger<ClientHandler> logger)
{

    private readonly Channel<IClientboundPacket> _clientboundChannel = Channel.CreateClientbound();

    private readonly Channel<IServerboundPacket> _serverboundChannel = Channel.CreateServerbound();

    private CancellationTokenSource? _cts;

    private Task? _transmitTask;

    private volatile bool _disconnecting;

    public event Action? Terminated;

    public Guid Id { get; } = Guid.CreateVersion7();

    public string Ip { get; } = ip;

    public TcpClient Client { get; } = client;

    public ServerService Server { get; } = server;

    public ClientInformationPacket? Information { get; set; }

    public ProtocolState State { get; private set; } = ProtocolState.Handshake;

    public async Task HandleAsync(CancellationToken cancellationToken)
    {
        await using var stream = Client.GetStream();
        await using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);
        using var _ = LogContext.PushProperty("ClientHandlerId", Id);
        LogClientConnected(this);

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _transmitTask = TransmitClientboundPacketsAsync(stream, writer, _cts.Token);
        var processTask = ProcessServerboundPacketsAsync(_cts.Token);

        try
        {
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
            await Task.WhenAll(_transmitTask, processTask);
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

    // TODO: outsource
    private async Task TransmitClientboundPacketsAsync(
        NetworkStream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        try
        {
            await foreach (var packet in _clientboundChannel.Reader.ReadAllAsync(cancellationToken))
            {
                await packetTransceiver.TransmitAsync(packet, stream, writer, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Shutting down
        }
        catch (Exception ex)
        {
            LogErrorWhileHandling(ex);
            await AbortAsync();
        }
    }

    // TODO: outsource
    private async Task ProcessServerboundPacketsAsync(CancellationToken cancellationToken)
    {
        try
        {
            await foreach (var packet in _serverboundChannel.Reader.ReadAllAsync(cancellationToken))
            {
                if (dispatcher.DispatchAsync(packet, this, cancellationToken) is { } handleTask)
                {
                    await handleTask;
                }
                else
                {
                    LogNoHandler(packet);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Shutting down
        }
        catch (Exception ex)
        {
            LogErrorWhileHandling(ex);
            await DisconnectAsync("Internal server error during packet processing");
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

        if (_transmitTask is not null)
        {
            await Task.WhenAny(_transmitTask, Task.Delay(2000));
        }

        await AbortAsync();
    }

    public Task AbortAsync()
    {
        return (_cts is { IsCancellationRequested: false })
            ? _cts.CancelAsync()
            : Task.CompletedTask;
    }

    public void Abort()
    {
        if (_cts is { IsCancellationRequested: false })
        {
            _cts.Cancel();
        }
    }

    private void Cleanup()
    {
        Client.Dispose();
        _cts?.Dispose();
        _cts = null;
        Terminated?.Invoke();
    }

    public override string ToString() => Ip;

}
