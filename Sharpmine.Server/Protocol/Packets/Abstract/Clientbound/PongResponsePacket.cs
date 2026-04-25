namespace Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

public abstract partial record PongResponsePacket
{

    public long Timestamp { get; init; }

    public void SerializeContent(Stream stream, BinaryWriter writer)
    {
        writer.Write(Timestamp);
    }

}
