using Microsoft.Extensions.Logging;

using Serilog.Context;

using Sharpmine.Server.Protocol.Packets;

namespace Sharpmine.Server.Protocol;

public sealed partial class ClientHandler(
    TcpClient client,
    ILogger<ClientHandler> logger) : IAsyncDisposable
{

    private CancellationTokenSource? _cts;

    private volatile bool _disposed;

    public event Action? Disposing;

    public event Action? Disposed;

    public Guid Id { get; } = Guid.CreateVersion7();

    public TcpClient Client { get; } = client;

    public PacketTransceiver PacketTransceiver { get; internal set; } = null!;

    public ProtocolState ProtocolState { get; private set; } = ProtocolState.Handshake;

    public void SwitchProtocolState(ProtocolState newState)
    {
        LogSwitchingState(ProtocolState, newState);
        ProtocolState = newState;
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
        var packet = await PacketTransceiver.ReceiveAsync(stream, reader, cancellationToken);

        if (packet is null)
        {
            return false;
        }

        try
        {
            await packet.ProcessAsync(this, stream, reader, writer, cancellationToken);
            return true;
        }
        catch (NotImplementedException)
        {
            LogProcessNotImplemented(packet);
            return false;
        }
    }

    public override string ToString()
    {
        return Client.Client.RemoteEndPoint?.ToString() ?? "<NULL>";
    }

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

    [LoggerMessage(LogLevel.Error, "{Packet} has no implementation for processing")]
    partial void LogProcessNotImplemented(IServerboundPacket packet);

    [LoggerMessage(LogLevel.Error, "An error occurred while handling the client")]
    partial void LogErrorWhileHandling(Exception error);

    [LoggerMessage(LogLevel.Information, "{Handler} connected")]
    partial void LogClientConnected(ClientHandler handler);

    [LoggerMessage(LogLevel.Information, "{Handler} disconnected")]
    partial void LogClientDisconnected(ClientHandler handler);

    [LoggerMessage(LogLevel.Information, "Connection to {Handler} was closed by server")]
    partial void LogClientWasDisconnected(ClientHandler handler);

    [LoggerMessage(LogLevel.Debug, "Switching state from {OldState} to {NewState}")]
    partial void LogSwitchingState(ProtocolState oldState, ProtocolState newState);

}
