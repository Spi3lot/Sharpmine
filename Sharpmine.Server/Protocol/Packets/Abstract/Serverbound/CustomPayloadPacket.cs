using Sharpmine.Server.Protocol.Attributes;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract partial record CustomPayloadPacket
{

    [PacketProperty]
    private string _channel;

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        _channel = reader.ReadString();
        throw new NotImplementedException("Data");
        return true;
    }

}
