using Sharpmine.Server.Core.Protocol.Packets.Abstract.Serverbound;

namespace Sharpmine.Server.Core.Protocol.Handlers.Abstract;

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
