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

    private readonly CancellationTokenSource _cts = new();

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
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cts.Token);
        LogClientConnected(this);

        try
        {
            // TODO: Check if Client.Connected is immediately false after Closing the Client when encountering a Lecacy Ping
            while (Client.Connected)
            {
                _ = await TryProcessNextPacketAsync(stream, reader, writer, linkedCts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            LogClientWasDisconnected(logger, this);
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
            linkedCts.Dispose();
            property.Dispose();
            await writer.DisposeAsync();
            reader.Dispose();
            await DisposeAsync();
        }
    }

    // TODO: Use ValueTask?
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

    // TODO: Fix throwing exception when calling LogClientDisconnected
    public override string ToString() => Client.Client.RemoteEndPoint!.ToString()!;

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
    static partial void LogClientWasDisconnected(ILogger<ClientHandler> logger, ClientHandler handler);

    [LoggerMessage(LogLevel.Debug, "Received {State}:0x{Id:X2} with {Length} bytes: {Packet}")]
    public partial void LogReceivedPacket(IServerboundPacket packet, ProtocolState state, int id, int length);

    public async ValueTask DisposeAsync()
    {
        Disposing?.Invoke();
        await _cts.CancelAsync();
        _cts.Dispose();
        Client.Dispose();
        Disposed?.Invoke();
    }

}
