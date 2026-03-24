using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Protocol.Packets;

// TODO: use System.IO.Pipelines for async IO
public partial class PacketTransceiver
{

    private readonly ILogger<PacketTransceiver> _logger;

    private readonly MemoryStream _memoryStream;

    private readonly BinaryWriter _memoryStreamWriter;

    public PacketTransceiver(ILogger<PacketTransceiver> logger)
    {
        _logger = logger;
        _memoryStream = new MemoryStream();
        _memoryStreamWriter = new BinaryWriter(_memoryStream);
    }

    public ClientHandler Handler { get; internal set; } = null!;

    // TODO: use System.IO.Pipelines for async reads
    public async Task TransmitAsync(
        IClientboundPacket packet,
        NetworkStream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        _memoryStream.SetLength(0);
        _memoryStreamWriter.Write7BitEncodedInt(packet.Id);

        try
        {
            await packet.SerializeContentAsync(_memoryStream, _memoryStreamWriter, cancellationToken);
        }
        catch (NotImplementedException)
        {
            LogSerializeNotImplemented(packet);
            return;
        }

        int packetLength = checked((int) _memoryStream.Length);
        writer.Write7BitEncodedInt(packetLength);
        await stream.WriteAsync(_memoryStream.GetBuffer().AsMemory(0, packetLength), cancellationToken);

        LogTransmittedPacket(packet, packet.State, packet.Id, packetLength);
    }

    public async Task<IServerboundPacket?> ReceiveAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        var state = Handler.ProtocolState;
        int length = reader.Read7BitEncodedInt();

        if (IsLegacyPing(state, length))
        {
            LogReceivedLegacyPing();
            await Handler.DisposeAsync();
            return null;
        }

        int packetId = reader.Read7BitEncodedInt();

        if (!ServerboundPacketRegistry.TryCreatePacket(state, packetId, out var packet))
        {
            LogReceivedUnknownPacket(state, packetId, length);
            return null;
        }

        try
        {
            await packet.DeserializeContentAsync(stream, reader, cancellationToken);
            LogReceivedPacket(packet, state, packetId, length);
            return packet;
        }
        catch (NotImplementedException)
        {
            LogDeserializeNotImplemented(packet);
            return null;
        }
    }

    /*
     * I am aware this check is not perfect and leads to a false positive
     * when an IntentionPacket with a length of 254 bytes is sent.
     * However for that to occur, the client would need to enter a hostname
     * of exactly 246 characters in length, which is highly unlikely to ever
     * be attempted by anyone, at least not in a serious manner.
     *
     * I therefore declare this a negligible side effect.
     */
    private static bool IsLegacyPing(ProtocolState state, int length)
    {
        // [0xFE, 0x01] => [0b_1111_1110, 0b_0000_0001] => 7BitEncodedInt => 0b1111_1110 => 0xFE => 254
        return state == ProtocolState.Handshake && length == 254;
    }

    [LoggerMessage(LogLevel.Error, "{Packet} has no implementation for serialization")]
    partial void LogSerializeNotImplemented(IClientboundPacket packet);

    [LoggerMessage(LogLevel.Error, "{Packet} has no implementation for deserialization")]
    partial void LogDeserializeNotImplemented(IServerboundPacket packet);

    [LoggerMessage(LogLevel.Error, "Received {State}:0x{Id:X2} with {Length} bytes: UNKNOWN PACKET")]
    partial void LogReceivedUnknownPacket(ProtocolState state, int id, int length);

    [LoggerMessage(LogLevel.Warning, "Received legacy ping, closing connection")]
    partial void LogReceivedLegacyPing();

    [LoggerMessage(LogLevel.Debug, "Transmitted {State}:0x{Id:X2} with {Length} bytes: {Packet}")]
    partial void LogTransmittedPacket(IClientboundPacket packet, ProtocolState state, int id, int length);

    [LoggerMessage(LogLevel.Debug, "Received {State}:0x{Id:X2} with {Length} bytes: {Packet}")]
    partial void LogReceivedPacket(IServerboundPacket packet, ProtocolState state, int id, int length);

}
