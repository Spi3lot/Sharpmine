using System.Net.Sockets;

using Serilog;

namespace Sharpmine.Server.Protocol.Packets;

public interface IServerboundPacket : IPacket
{

    Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader
    ) => throw new NotImplementedException();

    Task ProcessAsync(
        ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer
    ) => throw new NotImplementedException();

    static async Task<IServerboundPacket?> DeserializeAsync(
        ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader
    )
    {
        // TODO: use System.IO.Pipelines for async reads
        int length = reader.Read7BitEncodedInt();

        if (IsLegacyPing(handler.ProtocolState, length))
        {
            handler.LogReceivedLegacyPing();
            handler.Client.Close();
            return null;
        }

        int packetId = reader.Read7BitEncodedInt();

        if (!ServerboundPacketRegistry.TryCreatePacket(handler.ProtocolState, packetId, out var packet))
        {
            handler.LogReceivedUnknownPacket(handler.ProtocolState, packetId, length);
            return null;
        }

        try
        {
            await packet.DeserializeContentAsync(stream, reader);
            handler.LogReceivedPacket(packet, packet.State, packetId, length);
            return packet;
        }
        catch (NotImplementedException)
        {
            handler.LogNoImplementationForDeserialize(packet);
            return null;
        }
    }

    /*
     * I am aware this check is not perfect and leads to a false positive
     * when an IntentionPacket with the length of 254 bytes is sent.
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