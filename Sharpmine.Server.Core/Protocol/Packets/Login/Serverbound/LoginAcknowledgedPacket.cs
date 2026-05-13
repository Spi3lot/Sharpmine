using System.Buffers;

namespace Sharpmine.Server.Core.Protocol.Packets.Login.Serverbound;

public partial record LoginAcknowledgedPacket : IStateTransition, IHandlerless
{

    public ProtocolState NextState => ProtocolState.Configuration;

    public bool DeserializeContent(ref SequenceReader<byte> reader) => true;

}
