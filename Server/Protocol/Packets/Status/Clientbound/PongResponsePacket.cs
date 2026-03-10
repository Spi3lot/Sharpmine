using System.Net.Sockets;

namespace Sharpmine.Server.Protocol.Packets.Status.Clientbound;

public partial class PongResponsePacket
{

    public long Timestamp { get; set; }

    public Task SerializeContentAsync(NetworkStream stream, BinaryWriter writer)
    {
        writer.Write7BitEncodedInt64(Timestamp);
        return Task.CompletedTask;
    }

}