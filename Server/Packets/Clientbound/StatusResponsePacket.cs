using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Sharpmine.Server.Packets.Clientbound;

public class StatusResponsePacket : IClientboundPacket
{

    public int Id => 0x00;

    public required JsonObject JsonResponse { get; set; }

    public async Task SerializeAsync(NetworkStream stream, BinaryWriter writer)
    {
        await JsonSerializer.SerializeAsync(stream, JsonResponse);
    }

}