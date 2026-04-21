namespace Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

public abstract record KeepAlivePacket : IClientboundPacket
{

    ProtocolState IPacket.State => default;

    int IPacket.Id => 0;

    public long KeepAliveId { get; init; }

    public Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        writer.Write(KeepAliveId);
        return Task.CompletedTask;
    }

}
