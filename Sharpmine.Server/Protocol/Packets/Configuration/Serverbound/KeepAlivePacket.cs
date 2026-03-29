namespace Sharpmine.Server.Protocol.Packets.Configuration.Serverbound;

public partial record KeepAlivePacket
{

    public long KeepAliveId { get; set; }

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        KeepAliveId = reader.ReadInt64();
        return Task.CompletedTask;
    }

    public Task ProcessAsync(ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Check whether the client responded with the same packet");
    }

}
