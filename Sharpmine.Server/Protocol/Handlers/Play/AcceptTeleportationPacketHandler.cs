using Sharpmine.Server.Protocol.Packets.Play.Serverbound;

namespace Sharpmine.Server.Protocol.Handlers.Play;

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
