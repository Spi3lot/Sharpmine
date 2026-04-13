namespace Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

public abstract record PongResponsePacket : IClientboundPacket
{

    public abstract ProtocolState State { get; }

    public abstract int Id { get; }

    public long Timestamp { get; init; }

    public Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        writer.Write(Timestamp);
        return Task.CompletedTask;
    }

}
