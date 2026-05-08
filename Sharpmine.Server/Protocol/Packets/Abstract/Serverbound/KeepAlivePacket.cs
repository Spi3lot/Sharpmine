using Sharpmine.Server.Protocol.Attributes;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract partial record KeepAlivePacket
{

    [PacketProperty]
    private long _keepAliveId;

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        _keepAliveId = reader.ReadInt64();
        return true;
    }

}
