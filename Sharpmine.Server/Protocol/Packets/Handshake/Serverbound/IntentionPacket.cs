namespace Sharpmine.Server.Protocol.Packets.Handshake.Serverbound;

public partial record IntentionPacket : IStateTransition, IHandlerless
{

    public ProtocolState NextState => (Intent == Intent.Status) ? ProtocolState.Status : ProtocolState.Login;

    public int ProtocolVersion { get; set; }

    public string ServerAddress { get; set; } = null!;

    public ushort ServerPort { get; set; }

    public Intent Intent { get; set; }

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        ProtocolVersion = reader.Read7BitEncodedInt();
        ServerAddress = reader.ReadString();
        ServerPort = reader.ReadUInt16();
        Intent = (Intent) reader.Read7BitEncodedInt();
        return true;
    }

}

public enum Intent : byte
{

    Status = 1,

    Login = 2,

    Transfer = 3,

}
