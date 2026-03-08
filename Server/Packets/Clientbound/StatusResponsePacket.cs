using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Sharpmine.Server.Packets.Clientbound;

public class StatusResponsePacket : IClientboundPacket
{

    public int Id => 0x00;

    public JsonObject Status { get; set; } = null!;

    public Task SerializeAsync(NetworkStream stream, BinaryWriter writer)
    {
        return JsonSerializer.SerializeAsync(stream, Status);
    }

}