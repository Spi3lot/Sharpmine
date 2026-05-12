using System.Buffers;

using Sharpmine.Server.Protocol.Attributes;
using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

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
