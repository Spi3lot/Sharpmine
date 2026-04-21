using Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract record PingRequestPacket : IServerboundPacket
{

    ProtocolState IPacket.State => default;

    int IPacket.Id => 0;

    public long Timestamp { get; set; }

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        Timestamp = reader.ReadInt64();
        return Task.CompletedTask;
    }

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        handler.EnqueueClientboundPacket(CreatePongResponsePacket());
        return ValueTask.CompletedTask;
    }

    protected abstract PongResponsePacket CreatePongResponsePacket();

}
