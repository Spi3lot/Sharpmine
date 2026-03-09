using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets;

public interface IServerboundPacket
{

    Task DeserializeAsync(BinaryReader reader);

    Task ProcessAsync(
        ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer
    );

    static async Task<(IServerboundPacket? Packet, int PacketId, int Length)> DeserializeAsync(
        ClientHandler handler,
        BinaryReader reader
    )
    {
        // TODO: use System.IO.Pipelines for async reads
        int length = reader.Read7BitEncodedInt();
        int packetId = reader.Read7BitEncodedInt();
        var packet = ServerboundPacketRegistry.CreatePacket(packetId, handler.ProtocolState);

        if (packet is not null)
        {
            await packet.DeserializeAsync(reader);
        }

        return (packet, packetId, length);
    }

}