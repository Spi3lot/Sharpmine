using Optional;

using Sharpmine.Server.Protocol.DataTypes;
using Sharpmine.Server.Protocol.Packets.Login.Clientbound;
using Sharpmine.Server.Security;

using HelloPacket = Sharpmine.Server.Protocol.Packets.Login.Serverbound.HelloPacket;

namespace Sharpmine.Server.Protocol.Handlers.Login;

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

        // TODO: Send empty .Properties (only in offline-mode)
        var profile = new GameProfile(
            packet.Uuid,
            packet.Name,
            [new GameProfileProperty("textures", "1337", Option.Some("Singapore"))]);

        client.SendPacket(new LoginFinishedPacket(profile));
    }

}
