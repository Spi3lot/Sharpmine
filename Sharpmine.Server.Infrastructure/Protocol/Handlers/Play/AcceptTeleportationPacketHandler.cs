using Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Play;

public class AcceptTeleportationPacketHandler : IPacketHandler<AcceptTeleportationPacket>
{

    public ValueTask HandleAsync(
        AcceptTeleportationPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        // TODO: Implement
        return ValueTask.CompletedTask;
    }

}
