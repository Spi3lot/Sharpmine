using Sharpmine.Server.Infrastructure.Configuration;
using Sharpmine.Server.Infrastructure.Protocol.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Login.Clientbound;
using Sharpmine.Server.Infrastructure.Security;

using HelloPacket = Sharpmine.Server.Infrastructure.Protocol.Packets.Login.Serverbound.HelloPacket;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Login;

public class HelloPacketHandler(
    ServerProperties properties,
    PlayerAccessManager playerAccessManager,
    ServerCapacityManager serverCapacityManager) : IPacketHandler<HelloPacket>
{

    public async ValueTask HandleAsync(
        HelloPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        var (access, reason) = playerAccessManager.EvaluateAccess(client.Ip, packet.Uuid);

        if (access is not JoinAccess.Allowed)
        {
            await client.DisconnectAsync(reason!);
            return;
        }

        var player = new StatusPlayer(packet.Name, packet.Uuid);
        bool bypassesPlayerLimit = playerAccessManager.BypassesPlayerLimit(packet.Uuid);

        if (!serverCapacityManager.TryReserveSlot(client.Id, player, bypassesPlayerLimit))
        {
            await client.DisconnectAsync("Server is full!");
            return;
        }

        if (properties.OnlineMode)
        {
            // TODO
        }
        else
        {
            var profile = new GameProfile(packet.Uuid, packet.Name, []);
            client.SendPacket(new LoginFinishedPacket(in profile));
        }
    }

}
