using Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Serverbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Handlers.Abstract;

public class PingRequestPacketHandler : IPacketHandler<PingRequestPacket>
{

    public ValueTask HandleAsync(
        PingRequestPacket packet,
        ClientHandler client,
        CancellationToken cancellationToken)
    {
        client.SendPacket(packet.CreatePongResponsePacket());
        return ValueTask.CompletedTask;
    }

}
