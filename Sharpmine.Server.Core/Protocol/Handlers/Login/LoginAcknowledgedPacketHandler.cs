using Sharpmine.Server.Core.Domain;
using Sharpmine.Server.Core.Protocol.Packets.Configuration.Clientbound;
using Sharpmine.Server.Core.Protocol.Packets.Login.Serverbound;

namespace Sharpmine.Server.Core.Protocol.Handlers.Login;

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
