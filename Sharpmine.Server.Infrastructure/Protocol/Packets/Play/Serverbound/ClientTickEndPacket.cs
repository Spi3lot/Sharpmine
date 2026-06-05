using System.Buffers;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Serverbound;

public partial record ClientTickEndPacket
{

    public bool Log => false;

    public bool DeserializeContent(ref SequenceReader<byte> reader) => true;

}
