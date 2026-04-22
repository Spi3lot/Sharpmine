namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract record CustomPayloadPacket : IServerboundPacket
{

    ProtocolState IPacket.State => default;

    int IPacket.Id => 0;

    public string Channel { get; set; } = null!;

    // TODO: Data

    public ProtocolResult DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        Channel = reader.ReadString();
        return ProtocolResult.Success;
    }

    public ValueTask<ProtocolResult> ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(ProtocolResult.Success);
    }

}
