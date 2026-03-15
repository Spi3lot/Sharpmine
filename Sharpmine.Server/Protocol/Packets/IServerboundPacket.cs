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

}