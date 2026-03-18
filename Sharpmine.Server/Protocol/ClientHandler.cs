using System.Net.Sockets;

using Microsoft.Extensions.Logging;

using Serilog.Context;

using Sharpmine.Server.Protocol.Packets;

namespace Sharpmine.Server.Protocol;

public sealed partial class ClientHandler(
    TcpClient client,
    PacketSender packetSender,
    ILogger<ClientHandler> logger) : IAsyncDisposable
{

    private volatile bool _disposed;

    private CancellationTokenSource? _cts;

    public Guid Id { get; } = Guid.CreateVersion7();

    public TcpClient Client { get; } = client;

    public PacketSender PacketSender { get; } = packetSender;

    public ProtocolState ProtocolState { get; set; } = ProtocolState.Handshake;

    public event Action? Disposing;

    public event Action? Disposed;

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
            while (Client.Connected && !_cts.IsCancellationRequested)
            {
                _ = await TryProcessNextPacketAsync(stream, reader, writer, _cts.Token);
            }
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
            await DisposeAsync();
        }
    }

    // TODO: Test performance of Task vs ValueTask
    public async Task<bool> TryProcessNextPacketAsync(
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        var packet = await IServerboundPacket.DeserializeAsync(this, stream, reader, cancellationToken);

        if (packet is null)
        {
            return false;
        }

        await packet.ProcessAsync(this, stream, reader, writer, cancellationToken);
        return true;
    }

    public override string ToString() => Client.Client.RemoteEndPoint?.ToString() ?? "<NULL>";

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, true))
        {
            return;
        }

        Disposing?.Invoke();

        if (_cts is not null)
        {
            await _cts.CancelAsync();
            _cts.Dispose();
        }

        Client.Dispose();
        Disposed?.Invoke();
    }

    [LoggerMessage(LogLevel.Error, "Received {State}:0x{Id:X2} with {Length} bytes: UNKNOWN PACKET")]
    public partial void LogReceivedUnknownPacket(ProtocolState state, int id, int length);

    [LoggerMessage(LogLevel.Error, "{Packet} has no implementation for DeserializeContentAsync")]
    public partial void LogNoImplementationForDeserialize(IServerboundPacket packet);

    [LoggerMessage(LogLevel.Error, "An error occurred while handling the client")]
    partial void LogErrorWhileHandling(Exception error);

    [LoggerMessage(LogLevel.Warning, "Received legacy ping, closing connection")]
    public partial void LogReceivedLegacyPing();

    [LoggerMessage(LogLevel.Information, "{Handler} connected")]
    partial void LogClientConnected(ClientHandler handler);

    [LoggerMessage(LogLevel.Information, "{Handler} disconnected")]
    partial void LogClientDisconnected(ClientHandler handler);

    [LoggerMessage(LogLevel.Information, "Connection to {Handler} was closed by server")]
    partial void LogClientWasDisconnected(ClientHandler handler);

    [LoggerMessage(LogLevel.Debug, "Received {State}:0x{Id:X2} with {Length} bytes: {Packet}")]
    public partial void LogReceivedPacket(IServerboundPacket packet, ProtocolState state, int id, int length);

}
