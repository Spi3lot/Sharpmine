using Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract record PingRequestPacket : IServerboundPacket
{

    ProtocolState IPacket.State => default;

    int IPacket.Id => 0;

    public long Timestamp { get; set; }

    public ProtocolResult DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        Timestamp = reader.ReadInt64();
        return ProtocolResult.Success;
    }

    public ValueTask<ProtocolResult> ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        handler.EnqueueClientboundPacket(CreatePongResponsePacket());
        return ValueTask.FromResult(ProtocolResult.Success);
    }

    protected abstract PongResponsePacket CreatePongResponsePacket();

}
