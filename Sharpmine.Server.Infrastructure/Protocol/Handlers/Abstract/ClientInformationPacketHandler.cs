using Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Serverbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Abstract;

public class ClientInformationPacketHandler : IPacketHandler<ClientInformationPacket>
{

    public ValueTask HandleAsync(
        ClientInformationPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        client.Information = packet;
        return ValueTask.CompletedTask;
    }

}
