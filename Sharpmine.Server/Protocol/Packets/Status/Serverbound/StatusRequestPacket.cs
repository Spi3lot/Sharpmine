namespace Sharpmine.Server.Protocol.Packets.Status.Serverbound;

public partial record StatusRequestPacket
{

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader) => true;

}
