using Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Clientbound;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Login.Serverbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Login;

public class LoginAcknowledgedPacketHandler(RegistryCache registryCache) : IPacketHandler<LoginAcknowledgedPacket>
{

    public ValueTask HandleAsync(
        LoginAcknowledgedPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        foreach (var registryDataPacket in registryCache.Packets)
        {
            client.SendPacket(registryDataPacket);
        }

        client.SendPacket(new FinishConfigurationPacket());
        return ValueTask.CompletedTask;
    }

}
