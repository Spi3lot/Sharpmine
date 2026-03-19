using Sharpmine.Server.Protocol.Extensions;
using Sharpmine.Server.Protocol.Models;

namespace Sharpmine.Server.Protocol.Packets.Login.Clientbound;

public partial record LoginDisconnectPacket(TextComponent Reason)
{

    public Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        return stream.WriteJsonAsync(Reason, cancellationToken);
    }

}
