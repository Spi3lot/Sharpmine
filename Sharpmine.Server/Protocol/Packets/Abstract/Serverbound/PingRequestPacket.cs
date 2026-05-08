using Sharpmine.Server.Protocol.Attributes;
using Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract partial record PingRequestPacket
{

    [PacketProperty]
    private long _timestamp;

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        _timestamp = reader.ReadInt64();
        return true;
    }

    public abstract PongResponsePacket CreatePongResponsePacket();

}
