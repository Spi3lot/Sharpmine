using System.Net.Sockets;
using System.Text.Json;

namespace Sharpmine.Server.Protocol.Packets.Status.Clientbound;

public partial record StatusResponsePacket(in ServerStatus Status)
{

    public Task SerializeContentAsync(NetworkStream stream, BinaryWriter writer)
    {
        return JsonSerializer.SerializeAsync(stream, Status);
    }

}