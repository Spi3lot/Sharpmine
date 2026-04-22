namespace Sharpmine.Server.Protocol.Packets.Handshake.Serverbound;

public partial record IntentionPacket : IStateTransition
{

    public ProtocolState NextState => (Intent == Intent.Status) ? ProtocolState.Status : ProtocolState.Login;

    public int ProtocolVersion { get; set; }

    public string ServerAddress { get; set; } = null!;

    public ushort ServerPort { get; set; }

    public Intent Intent { get; set; }

    public ProtocolResult DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        ProtocolVersion = reader.Read7BitEncodedInt();
        ServerAddress = reader.ReadString();
        ServerPort = reader.ReadUInt16();
        Intent = (Intent) reader.Read7BitEncodedInt();
        return ProtocolResult.Success;
    }

    public ValueTask<ProtocolResult> ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(ProtocolResult.Success);
    }

}

public enum Intent : byte
{

    Status = 1,

    Login = 2,

    Transfer = 3,

}
