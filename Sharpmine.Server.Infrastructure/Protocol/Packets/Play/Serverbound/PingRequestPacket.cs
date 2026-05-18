using Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Clientbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

public partial record PingRequestPacket
{

    public override PongResponsePacket CreatePongResponsePacket()
    {
        return new Clientbound.PongResponsePacket { Timestamp = Timestamp };
    }

}
