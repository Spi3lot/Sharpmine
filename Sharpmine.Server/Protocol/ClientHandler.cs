using System.Text;
using System.Threading.Channels;

using Microsoft.Extensions.Logging;

using Serilog.Context;

using Sharpmine.Server.Protocol.Extensions;
using Sharpmine.Server.Protocol.Packets;
using Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

namespace Sharpmine.Server.Protocol;

public sealed partial class ClientHandler(
    TcpClient client,
    ServerService server,
    PacketTransceiver packetTransceiver,
    ILogger<ClientHandler> logger) : IDisposable
{

    private readonly Channel<IClientboundPacket> _clientboundChannel = Channel.CreateClientbound();

    private readonly Channel<IServerboundPacket> _serverboundChannel = Channel.CreateServerbound();

    private CancellationTokenSource? _cts;

    private volatile bool _disposed;

    public event Action? Disposed;

    public Guid Id { get; } = Guid.CreateVersion7();

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
        var writeTask = TransmitClientboundPacketsAsync(stream, writer, _cts.Token);
        var processTask = ProcessServerboundPacketsAsync(_cts.Token);

        try
        {
            while (!_cts.IsCancellationRequested
                   && await TryEnqueueNextServerboundPacketAsync(stream, reader, _cts.Token)) ;
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
            throw;
        }
        finally
        {
            await DisconnectAsync();
            await Task.WhenAll(writeTask, processTask);
            Dispose();
        }
    }

    private async Task<bool> TryEnqueueNextServerboundPacketAsync(
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

    public void EnqueueClientboundPacket(IClientboundPacket packet)
    {
        if (!_clientboundChannel.Writer.TryWrite(packet))
        {
            LogDisconnectingClient(this, "Too many clientbound packets queued");
            Disconnect();
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
            await DisconnectAsync();
        }
    }

    // TODO: outsource
    private async Task ProcessServerboundPacketsAsync(CancellationToken cancellationToken)
    {
        try
        {
            await foreach (var packet in _serverboundChannel.Reader.ReadAllAsync(cancellationToken))
            {
                try
                {
                    await packet.ProcessAsync(this, cancellationToken);
                }
                catch (NotImplementedException)
                {
                    LogProcessNotImplemented(packet);
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
            await DisconnectAsync();
        }
    }

    public override string ToString()
    {
        return Client.Client.RemoteEndPoint?.ToString() ?? "<NULL>";
    }

    public void Disconnect()
    {
        if (_cts is { IsCancellationRequested: false })
        {
            _cts.Cancel();
        }
    }

    public Task DisconnectAsync()
    {
        return (_cts is { IsCancellationRequested: false })
            ? _cts.CancelAsync()
            : Task.CompletedTask;
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, true))
        {
            return;
        }

        Client.Dispose();
        _cts?.Dispose();
        _cts = null;
        Disposed?.Invoke();
    }

}
