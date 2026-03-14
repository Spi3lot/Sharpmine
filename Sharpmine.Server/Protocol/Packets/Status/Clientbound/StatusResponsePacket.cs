using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Sharpmine.Server.Protocol.Packets.Status.Clientbound;

public partial record StatusResponsePacket(JsonObject Status)
{

    public Task SerializeContentAsync(NetworkStream stream, BinaryWriter writer)
    {
        return JsonSerializer.SerializeAsync(stream, Status);
    }

}