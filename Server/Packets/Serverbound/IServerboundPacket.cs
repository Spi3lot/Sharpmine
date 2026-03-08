using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Sharpmine.Server.Packets.Serverbound;

public interface IServerboundPacket
{

    Task Deserialize(BinaryReader reader);

    Task Process(ClientHandler handler, BinaryReader reader, BinaryWriter writer);

    static async Task<(IServerboundPacket? Packet, int PacketId, int Length)> TryDeserialize(BinaryReader reader)
    {
        int length = reader.Read7BitEncodedInt();
        int packetId = reader.Read7BitEncodedInt();

        if (!ServerboundPacketRegistry.TryCreatePacketById(packetId, out var packet))
        {
            return (null, packetId, length);
        }

        await packet.Deserialize(reader);
        return (packet, packetId, length);
    }

}