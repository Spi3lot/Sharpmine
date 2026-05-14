using System.Buffers;

namespace Sharpmine.Server.Core.Protocol.Packets.Play.Serverbound;

public partial record ConfigurationAcknowledgedPacket : IStateTransition, IHandlerless
{

    public ProtocolState NextState => ProtocolState.Configuration;

    public bool DeserializeContent(ref SequenceReader<byte> reader) => true;

}
