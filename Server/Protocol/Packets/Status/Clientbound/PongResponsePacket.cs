using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets.Status.Clientbound;

public class PongResponsePacket : IClientboundPacket
{

    public int Id => 0x01;

    public long Timestamp { get; set; }

    public Task SerializeAsync(NetworkStream stream, BinaryWriter writer)
    {
        writer.Write7BitEncodedInt64(Timestamp);
        return Task.CompletedTask;
    }

}