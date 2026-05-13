using System.Buffers;

using Sharpmine.Server.Protocol.Attributes;
using Sharpmine.Server.Protocol.Extensions;

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

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadVarInt(out _protocolVersion)
               && reader.TryReadString(out _serverAddress, 255)
               && reader.TryReadUInt16(out _serverPort)
               && reader.TryReadEnum(out _intent);
    }

}

public enum Intent
{

    Status = 1,

    Login = 2,

    Transfer = 3,

}
