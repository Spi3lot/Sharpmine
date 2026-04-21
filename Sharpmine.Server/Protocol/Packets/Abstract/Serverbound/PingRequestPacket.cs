using Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract record PingRequestPacket : IServerboundPacket
{

    public abstract ProtocolState State { get; }

    public abstract int Id { get; }

    public long Timestamp { get; set; }

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        Timestamp = reader.Read7BitEncodedInt64();
        return Task.CompletedTask;
    }

    public async ValueTask ProcessAsync(
        ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        handler.EnqueueClientboundPacket(CreatePongResponsePacket());
        await handler.DisposeAsync();
    }

    protected abstract PongResponsePacket CreatePongResponsePacket();

}
