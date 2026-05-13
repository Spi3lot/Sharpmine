using Sharpmine.Server.Core.Protocol.Packets.Play.Serverbound;

namespace Sharpmine.Server.Core.Protocol.Handlers.Play;

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
