using Sharpmine.Server.Core.Protocol.Packets;

namespace Sharpmine.Server.Core.Protocol.Handlers;

public interface IPacketHandler<in T> where T : IServerboundPacket
{

    ValueTask HandleAsync(
        T packet,
        ClientHandler client,
        CancellationToken cancellationToken);

}
