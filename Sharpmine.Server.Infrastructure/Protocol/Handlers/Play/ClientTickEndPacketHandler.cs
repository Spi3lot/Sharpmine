using Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Play;

public class ClientTickEndPacketHandler : IPacketHandler<ClientTickEndPacket>
{

    public ValueTask HandleAsync(
        ClientTickEndPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken) => ValueTask.CompletedTask;

}
