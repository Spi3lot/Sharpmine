using System.Buffers;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Status.Serverbound;

public partial record StatusRequestPacket
{

    public bool DeserializeContent(ref SequenceReader<byte> reader) => true;

}
