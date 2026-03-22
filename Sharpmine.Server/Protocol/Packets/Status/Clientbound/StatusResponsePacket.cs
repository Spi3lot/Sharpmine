using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Status.Clientbound;

public partial record StatusResponsePacket(ServerStatus Status)
{

    public async Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        var memoryStream = new MemoryStream();
        await memoryStream.WriteJsonAsync(Status, cancellationToken);
        short length = checked((short) memoryStream.Length);
        writer.Write7BitEncodedInt(length);
        await stream.WriteAsync(memoryStream.GetBuffer().AsMemory(0, length), cancellationToken);
    }

}
