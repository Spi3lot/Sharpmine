namespace Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

public abstract record KeepAlivePacket : IClientboundPacket
{

    public abstract ProtocolState State { get; }

    public abstract int Id { get; }

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
