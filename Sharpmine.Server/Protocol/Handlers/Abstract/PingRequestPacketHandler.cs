using Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

namespace Sharpmine.Server.Protocol.Handlers.Abstract;

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
