namespace Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

public abstract record PongResponsePacket : IClientboundPacket
{

    ProtocolState IPacket.State => default;

    int IPacket.Id => 0;

    public long Timestamp { get; init; }

    public void SerializeContent(Stream stream, BinaryWriter writer)
    {
        writer.Write(Timestamp);
    }

}
