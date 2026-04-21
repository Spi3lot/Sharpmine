namespace Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

public abstract record PongResponsePacket : IClientboundPacket
{

    ProtocolState IPacket.State => default;

    int IPacket.Id => 0;

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
