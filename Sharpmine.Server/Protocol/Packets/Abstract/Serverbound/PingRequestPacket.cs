using Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract record PingRequestPacket : IServerboundPacket
{

    ProtocolState IPacket.State => default;

    int IPacket.Id => 0;

    public long Timestamp { get; set; }

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        Timestamp = reader.ReadInt64();
        return true;
    }

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        handler.SendPacket(CreatePongResponsePacket());
        return ValueTask.CompletedTask;
    }

    protected abstract PongResponsePacket CreatePongResponsePacket();

}
