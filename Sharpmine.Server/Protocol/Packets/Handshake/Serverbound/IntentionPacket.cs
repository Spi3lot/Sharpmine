using Sharpmine.Server.Protocol.Attributes;

namespace Sharpmine.Server.Protocol.Packets.Handshake.Serverbound;

public partial record IntentionPacket : IStateTransition, IHandlerless
{

    public ProtocolState NextState => (_intent == Intent.Status) ? ProtocolState.Status : ProtocolState.Login;

    [PacketProperty]
    private int _protocolVersion;

    [PacketProperty]
    private string _serverAddress;

    [PacketProperty]
    private ushort _serverPort;

    [PacketProperty]
    private Intent _intent;

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        _protocolVersion = reader.Read7BitEncodedInt();
        _serverAddress = reader.ReadString();
        _serverPort = reader.ReadUInt16();
        _intent = (Intent) reader.Read7BitEncodedInt();
        return true;
    }

}

public enum Intent : byte
{

    Status = 1,

    Login = 2,

    Transfer = 3,

}
