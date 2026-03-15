using System.Net.Sockets;

using Microsoft.Extensions.Logging;

using Serilog.Context;

using Sharpmine.Server.Protocol.Packets;

namespace Sharpmine.Server.Protocol;

public partial class ClientHandler(
    TcpClient client,
    PacketSender packetSender,
    ILogger<ClientHandler> logger
)
{

    public Guid Id { get; } = Guid.CreateVersion7();

    public TcpClient Client { get; } = client;

    public PacketSender PacketSender { get; } = packetSender;

    public ProtocolState ProtocolState { get; set; } = ProtocolState.Handshake;

    public event Action? ConnectionTerminated;

    public async Task HandleAsync()
    {
        var stream = Client.GetStream();
        var reader = new BinaryReader(stream);
        var writer = new BinaryWriter(stream);
        var property = LogContext.PushProperty("ClientHandlerId", Id);

        try
        {
            while (Client.Connected)
            {
                _ = await TryProcessNextPacketAsync(stream, reader, writer);
            }
        }
        catch (IOException)
        {
            LogClientDisconnected(this);
        }
        catch (Exception ex)
        {
            LogException(ex);
            throw;
        }
        finally
        {
            property.Dispose();
            await writer.DisposeAsync();
            reader.Dispose();
            Client.Dispose();
            ConnectionTerminated?.Invoke();
        }
    }

    public async Task<bool> TryProcessNextPacketAsync(NetworkStream stream, BinaryReader reader, BinaryWriter writer)
    {
        var packet = await IServerboundPacket.DeserializeAsync(this, stream, reader);

        if (packet is null)
        {
            return false;
        }

        await packet.ProcessAsync(this, stream, reader, writer);
        return true;
    }

    public override string ToString() => Client.Client.RemoteEndPoint!.ToString()!;

    [LoggerMessage(LogLevel.Error, "Received unknown packet ({State}:0x{Id:X2}, {Length} bytes)")]
    public partial void LogReceivedUnknownPacket(ProtocolState state, int id, int length);

    [LoggerMessage(LogLevel.Error, "{Packet} has no implementation for DeserializeContentAsync")]
    public partial void LogNoImplementationForDeserialize(IServerboundPacket packet);

    [LoggerMessage(LogLevel.Error)]
    partial void LogException(Exception error);

    [LoggerMessage(LogLevel.Information, "{Handler} disconnected")]
    partial void LogClientDisconnected(ClientHandler handler);

    [LoggerMessage(LogLevel.Debug, "Received {Packet} ({State}:0x{Id:X2}, {Length} bytes)")]
    public partial void LogReceivedPacket(IServerboundPacket packet, ProtocolState state, int id, int length);

}