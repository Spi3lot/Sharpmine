using Sharpmine.Server.Core.Protocol.Packets.Abstract.Serverbound;

namespace Sharpmine.Server.Core.Protocol.Handlers.Abstract;

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
