using Sharpmine.Domain.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Play;

public class MovePlayerRotPacketHandler : IPacketHandler<MovePlayerRotPacket>
{

    public ValueTask HandleAsync(
        MovePlayerRotPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        client.Player!.Rotation = new PlayerRotation(packet.Yaw, packet.Pitch);
        return ValueTask.CompletedTask;
    }

}
