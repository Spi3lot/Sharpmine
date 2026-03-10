using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets;

public interface IServerboundPacket : IPacket
{

    Task DeserializeContentAsync(BinaryReader reader) => throw new NotImplementedException();

    Task ProcessAsync(
        ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer
    ) => throw new NotImplementedException();

    static async Task<IServerboundPacket?> DeserializeAsync(
        ClientHandler handler,
        BinaryReader reader
    )
    {
        // TODO: use System.IO.Pipelines for async reads
        int length = reader.Read7BitEncodedInt();
        int packetId = reader.Read7BitEncodedInt();

        if (!ServerboundPacketRegistry.TryCreatePacket(handler.ProtocolState, packetId, out var packet))
        {
            await Console.Error.WriteLineAsync($"Huh? Received unknown packet ({handler.ProtocolState}:0x{packetId:X2}) consisting of {length} bytes");
            return null;
        }

        await packet.DeserializeContentAsync(reader);
        return packet;
    }

}