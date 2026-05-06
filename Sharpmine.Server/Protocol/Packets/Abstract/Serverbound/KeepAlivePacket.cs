namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract partial record KeepAlivePacket
{

    public long KeepAliveId { get; set; }

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        KeepAliveId = reader.ReadInt64();
        return true;
    }

}
