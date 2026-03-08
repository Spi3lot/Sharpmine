using System.Diagnostics.CodeAnalysis;

namespace Sharpmine.Server.Packets.Serverbound;

public interface IServerboundPacket
{

    void Deserialize(BinaryReader reader);

    void Process(ClientHandler handler, BinaryReader reader, BinaryWriter writer);

    public static bool TryDeserialize(
        BinaryReader reader,
        [NotNullWhen(true)] out IServerboundPacket? packet
    )
    {
        return TryDeserialize(reader, out packet, out _, out _);
    }

    public static bool TryDeserialize(
        BinaryReader reader,
        [NotNullWhen(true)] out IServerboundPacket? packet,
        out int packetId,
        out int length
    )
    {
        length = reader.Read7BitEncodedInt();
        packetId = reader.Read7BitEncodedInt();

        if (!ServerboundPacketRegistry.TryCreatePacketById(packetId, out packet))
        {
            return false;
        }
        
        packet.Deserialize(reader);
        return true;
    }

}