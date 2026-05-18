using System.Buffers;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Login.Serverbound;

public partial record LoginAcknowledgedPacket : IStateTransition
{

    public ProtocolState NextState => ProtocolState.Configuration;

    public bool DeserializeContent(ref SequenceReader<byte> reader) => true;

}
