using System.Text.Json;
using System.Text.Json.Nodes;

namespace Sharpmine.Server.Packets.Clientbound;

public class StatusResponsePacket : IClientboundPacket
{

    public int Id => 0x00;

    public required JsonObject Response { get; set; }

    public async Task Serialize(BinaryWriter writer)
    {
        await JsonSerializer.SerializeAsync(writer.BaseStream, Response);
    }

}