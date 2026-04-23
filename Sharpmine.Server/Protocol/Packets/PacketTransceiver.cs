using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Protocol.Packets;

public partial class PacketTransceiver
{

#pragma warning disable once S4487
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
            packet.SerializeContent(_memoryStream, _memoryStreamWriter);
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

    public async Task<(bool KeepAlive, IServerboundPacket? Packet)> ReceiveAsync(
        ProtocolState state,
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested(); // TODO: Remove once Pipelines are implemented
        int length = reader.Read7BitEncodedInt();

        if (length == 0)
        {
            LogReceivedEmptyPacket(state);
            return (false, null);
        }

        if (IsLegacyPing(state, length))
        {
            LogReceivedLegacyPing();
            return (false, null);
        }

        int packetId = reader.Read7BitEncodedInt();

        if (!ServerboundPacketRegistry.TryCreatePacket(state, packetId, out var packet))
        {
            LogReceivedUnknownPacket(state, packetId, length);
            return (false, null); // TODO: Until Pipelines are implemented to safely advance the buffer, we MUST drop the connection.
        }

        bool success;

        try
        {
            success = packet.DeserializeContent(stream, reader);
        }
        catch (NotImplementedException)
        {
            // TODO: 'KeepAlive=packet is not IStateTransition' when using Pipelines
            //       Because we didn't read the payload bytes, the TCP stream is desynced.
            LogDeserializeNotImplemented(packet);
            return (false, null);
        }

        if (!success)
        {
            LogDeserializeViolation(packet);
            return (false, null);
        }

        LogReceivedPacket(packet, state, packetId, length);
        return (true, packet);
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

}
