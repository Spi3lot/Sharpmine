using Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Play;

public class ChangeGameModePacketHandler : IPacketHandler<ChangeGameModePacket>
{

    public ValueTask HandleAsync(
        ChangeGameModePacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        // TODO: Secure
        client.SendPacket(GameEventPacket.ChangeGameMode(packet.GameMode));
        return ValueTask.CompletedTask;
    }

}
