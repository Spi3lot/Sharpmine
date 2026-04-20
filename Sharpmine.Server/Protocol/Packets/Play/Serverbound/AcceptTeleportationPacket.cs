namespace Sharpmine.Server.Protocol.Packets.Play.Serverbound;

public partial record AcceptTeleportationPacket
{

    public int TeleportId { get; set; }

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        TeleportId = reader.Read7BitEncodedInt();
        return Task.CompletedTask;
    }

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

}
