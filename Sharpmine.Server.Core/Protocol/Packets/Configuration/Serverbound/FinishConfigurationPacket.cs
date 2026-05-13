using System.Buffers;

namespace Sharpmine.Server.Core.Protocol.Packets.Configuration.Serverbound;

public partial record FinishConfigurationPacket : IStateTransition, IHandlerless
{

    public ProtocolState NextState => ProtocolState.Play;

    public bool DeserializeContent(ref SequenceReader<byte> reader) => true;

}
