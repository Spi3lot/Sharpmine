namespace Sharpmine.Server.Protocol.Packets.Play.Serverbound;

public partial record AcceptTeleportationPacket
{

    public int TeleportId { get; set; }

    public ProtocolResult DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        TeleportId = reader.Read7BitEncodedInt();
        return ProtocolResult.Success;
    }

    public ValueTask<ProtocolResult> ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(ProtocolResult.NotImplemented);
    }

}
