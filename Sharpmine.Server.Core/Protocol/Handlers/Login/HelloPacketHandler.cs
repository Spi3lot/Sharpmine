using Optional;

using Sharpmine.Server.Core.Protocol.DataTypes;
using Sharpmine.Server.Core.Protocol.Packets.Login.Clientbound;
using Sharpmine.Server.Core.Security;

using HelloPacket = Sharpmine.Server.Core.Protocol.Packets.Login.Serverbound.HelloPacket;

namespace Sharpmine.Server.Core.Protocol.Handlers.Login;

public class HelloPacketHandler(
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

        if (!serverCapacityManager.TryReserveSlot(playerAccessManager.BypassesPlayerLimit(packet.Uuid)))
        {
            await client.DisconnectAsync("Server is full!");
            return;
        }

        client.OccupiesPlayerSlot = true;

        // TODO: Send empty .Properties (only in offline-mode)
        var profile = new GameProfile(
            packet.Uuid,
            packet.Name,
            [new GameProfileProperty("textures", "1337", Option.Some("Singapore"))]);

        client.SendPacket(new LoginFinishedPacket(profile));
    }

}
