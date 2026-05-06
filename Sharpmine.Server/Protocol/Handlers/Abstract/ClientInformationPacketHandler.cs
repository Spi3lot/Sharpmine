using Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

namespace Sharpmine.Server.Protocol.Handlers.Abstract;

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
