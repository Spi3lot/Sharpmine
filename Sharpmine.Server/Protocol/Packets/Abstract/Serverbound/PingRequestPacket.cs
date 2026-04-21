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
        Timestamp = reader.Read7BitEncodedInt64();
        return Task.CompletedTask;
    }

    public async ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        handler.EnqueueClientboundPacket(CreatePongResponsePacket());
        await handler.DisposeAsync();
    }

    protected abstract PongResponsePacket CreatePongResponsePacket();

}
