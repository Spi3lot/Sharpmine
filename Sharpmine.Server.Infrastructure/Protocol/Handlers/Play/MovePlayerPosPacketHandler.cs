using Sharpmine.Domain.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Play;

public class MovePlayerPosPacketHandler : IPacketHandler<MovePlayerPosPacket>
{

    public ValueTask HandleAsync(
        MovePlayerPosPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        client.Player!.Position = new PlayerPosition(packet.X, packet.FeetY, packet.Z);
        return ValueTask.CompletedTask;
    }

}
