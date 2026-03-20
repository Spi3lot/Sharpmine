using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Status.Clientbound;

public partial record StatusResponsePacket(ServerStatus Status)
{

    public Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        return stream.WriteJsonAsync(Status, cancellationToken);
    }

}
