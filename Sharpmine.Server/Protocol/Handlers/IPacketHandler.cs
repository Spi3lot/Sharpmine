using Sharpmine.Server.Protocol.Packets;

namespace Sharpmine.Server.Protocol.Handlers;

public interface IPacketHandler<in T> where T : IServerboundPacket
{

    ValueTask HandleAsync(T packet, ClientHandler client, CancellationToken cancellationToken);

}
