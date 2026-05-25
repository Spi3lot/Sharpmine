using System.Buffers;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Configuration.Serverbound;

public partial record FinishConfigurationPacket : IStateTransition
{

    public ProtocolState NextState => ProtocolState.Play;

    public bool DeserializeContent(ref SequenceReader<byte> reader) => true;

}
