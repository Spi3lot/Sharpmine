using Sharpmine.Domain.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Play;

public class MovePlayerPosRotPacketHandler : IPacketHandler<MovePlayerPosRotPacket>
{

    public ValueTask HandleAsync(
        MovePlayerPosRotPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        client.Player!.Position = new PlayerPosition(packet.X, packet.FeetY, packet.Z);
        client.Player!.Rotation = new PlayerRotation(packet.Yaw, packet.Pitch);
        return ValueTask.CompletedTask;
    }

}
