using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Attributes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Serverbound;

public abstract partial record CustomPayloadPacket
{

    [PacketProperty]
    private string _channel;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadString(out _channel);
        throw new NotImplementedException("Data");
    }

}
