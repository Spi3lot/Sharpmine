namespace Sharpmine.Server.Protocol.Packets.Play.Serverbound;

public partial record AcceptTeleportationPacket
{

    public int TeleportId { get; set; }

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        TeleportId = reader.Read7BitEncodedInt();
        return true;
    }

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

}
