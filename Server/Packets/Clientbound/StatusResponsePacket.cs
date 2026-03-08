using System.Text.Json;
using System.Text.Json.Nodes;

namespace Sharpmine.Server.Packets.Clientbound;

public class StatusResponsePacket : IClientboundPacket
{

    public int Id => 0x00;

    public JsonObject Response { get; set; }

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(JsonSerializer.Serialize(Response));
    }

}