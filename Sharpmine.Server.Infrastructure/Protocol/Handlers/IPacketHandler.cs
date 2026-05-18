using Sharpmine.Server.Infrastructure.Protocol.Packets;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers;

public interface IPacketHandler<in T> where T : IServerboundPacket
{

    ValueTask HandleAsync(
        T packet,
        ClientHandler client,
        CancellationToken cancellationToken);

}
