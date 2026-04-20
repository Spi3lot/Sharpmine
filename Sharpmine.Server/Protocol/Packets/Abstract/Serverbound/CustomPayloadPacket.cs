namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract record CustomPayloadPacket : IServerboundPacket
{

    public abstract ProtocolState State { get; }

    public abstract int Id { get; }

    public string Channel { get; set; } = null!;

    // TODO: Data

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        Channel = reader.ReadString();
        return Task.CompletedTask;
    }

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken) => ValueTask.CompletedTask;

}
