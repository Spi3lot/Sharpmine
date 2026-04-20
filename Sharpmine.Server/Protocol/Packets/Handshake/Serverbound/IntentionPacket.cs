namespace Sharpmine.Server.Protocol.Packets.Handshake.Serverbound;

public partial record IntentionPacket
{

    public int ProtocolVersion { get; set; }

    public string ServerAddress { get; set; } = null!;

    public ushort ServerPort { get; set; }

    public Intent Intent { get; set; }

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        ProtocolVersion = reader.Read7BitEncodedInt();
        ServerAddress = reader.ReadString();
        ServerPort = reader.ReadUInt16();
        Intent = (Intent) reader.Read7BitEncodedInt();
        return Task.CompletedTask;
    }

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        handler.SwitchProtocolState((Intent == Intent.Status) ? ProtocolState.Status : ProtocolState.Login);
        return ValueTask.CompletedTask;
    }

}

public enum Intent : byte
{

    Status = 1,

    Login = 2,

    Transfer = 3,

}
