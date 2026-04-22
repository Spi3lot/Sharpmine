namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract record KeepAlivePacket : IServerboundPacket
{

    ProtocolState IPacket.State => default;

    int IPacket.Id => 0;

    public long KeepAliveId { get; set; }

    public ProtocolResult DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        KeepAliveId = reader.ReadInt64();
        return ProtocolResult.Success;
    }

    public ValueTask<ProtocolResult> ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        // TODO: Check whether the client responded with the same packet
        return ValueTask.FromResult(ProtocolResult.NotImplemented);
    }

}
