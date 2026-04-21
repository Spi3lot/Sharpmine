namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract record KeepAlivePacket : IServerboundPacket
{

    ProtocolState IPacket.State => default;

    int IPacket.Id => 0;

    public long KeepAliveId { get; set; }

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        KeepAliveId = reader.ReadInt64();
        return Task.CompletedTask;
    }

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Check whether the client responded with the same packet");
    }

}
