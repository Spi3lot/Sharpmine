using Sharpmine.Server.Protocol.Attributes;
using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record HelloPacket
{

    [PacketProperty]
    private string _name;

    [PacketProperty]
    private Guid _uuid;

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        _name = reader.ReadString();
        _uuid = reader.ReadUuid();
        return true;
    }

}
