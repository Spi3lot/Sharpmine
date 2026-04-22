namespace Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

public abstract record KeepAlivePacket : IClientboundPacket
{

    ProtocolState IPacket.State => default;

    int IPacket.Id => 0;

    public long KeepAliveId { get; init; }

    public void SerializeContent(Stream stream, BinaryWriter writer)
    {
        writer.Write(KeepAliveId);
    }

}
