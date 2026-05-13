using Sharpmine.Server.Core.Protocol.Packets.Abstract.Clientbound;

namespace Sharpmine.Server.Core.Protocol.Packets.Status.Serverbound;

public partial record PingRequestPacket
{

    public override PongResponsePacket CreatePongResponsePacket()
    {
        return new Clientbound.PongResponsePacket { Timestamp = Timestamp };
    }

}
