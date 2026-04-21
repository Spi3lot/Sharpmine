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

    public void TransitionTo(ProtocolState newState)
    {
        LogStateTransition(State, newState);
        State = newState;
    }

    public async Task HandleAsync(CancellationToken cancellationToken)
    {
        var stream = Client.GetStream();
        var reader = new BinaryReader(stream);
        var writer = new BinaryWriter(stream);
        var property = LogContext.PushProperty("ClientHandlerId", Id);
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        LogClientConnected(this);

        try
        {
            var writeTask = TransmitClientboundPacketsAsync(stream, writer, _cts.Token);
            var processTask = ProcessServerboundPacketsAsync(_cts.Token);

            while (Client.Connected
                   && !_cts.IsCancellationRequested
                   && await TryEnqueueNextServerboundPacketAsync(stream, reader, _cts.Token)) ;

            await _cts.CancelAsync();
            await Task.WhenAll(writeTask, processTask);
        }
        catch (OperationCanceledException)
        {
            LogClientWasDisconnected(this);
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
            property.Dispose();
            await writer.DisposeAsync();
            reader.Dispose();
            Dispose();
        }
    }

    private async Task<bool> TryEnqueueNextServerboundPacketAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        var packet = await packetTransceiver.ReceiveAsync(State, stream, reader, cancellationToken);
        return packet is not null && _serverboundChannel.Writer.TryWrite(packet);
    }

    public void EnqueueClientboundPacket(IClientboundPacket packet)
    {
        // ReSharper disable once InvertIf
        if (!_clientboundChannel.Writer.TryWrite(packet))
        {
            logger.LogWarning("Disconnecting {Handler}: Too many clientbound packets queued.", this);
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
