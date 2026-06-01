using Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Play;

public class PlayerLoadedPacketHandler : IPacketHandler<PlayerLoadedPacket>
{

    public ValueTask HandleAsync(
        PlayerLoadedPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

}
