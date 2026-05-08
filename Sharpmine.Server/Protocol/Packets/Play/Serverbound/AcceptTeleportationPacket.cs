using Sharpmine.Server.Protocol.Attributes;

namespace Sharpmine.Server.Protocol.Packets.Play.Serverbound;

public partial record AcceptTeleportationPacket
{

    [PacketProperty]
    private int _teleportId;

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        _teleportId = reader.Read7BitEncodedInt();
        return true;
    }

}
