using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets.Status.Clientbound;

public partial record PongResponsePacket(long Timestamp)
{

    public Task SerializeContentAsync(NetworkStream stream, BinaryWriter writer)
    {
        writer.Write7BitEncodedInt64(Timestamp);
        return Task.CompletedTask;
    }

}