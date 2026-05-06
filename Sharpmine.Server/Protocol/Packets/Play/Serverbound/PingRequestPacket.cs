using Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Play.Serverbound;

public partial record PingRequestPacket
{

    public override PongResponsePacket CreatePongResponsePacket()
    {
        return new Clientbound.PongResponsePacket { Timestamp = Timestamp };
    }

}
