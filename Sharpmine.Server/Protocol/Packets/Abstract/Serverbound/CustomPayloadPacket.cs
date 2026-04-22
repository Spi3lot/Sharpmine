namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract record CustomPayloadPacket : IServerboundPacket
{

    ProtocolState IPacket.State => default;

    int IPacket.Id => 0;

    public string Channel { get; set; } = null!;

    // TODO: Data

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        Channel = reader.ReadString();
        return true;
    }

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

}
