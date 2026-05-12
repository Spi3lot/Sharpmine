using System.Buffers;

using Sharpmine.Server.Protocol.Attributes;
using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract partial record KeepAlivePacket
{

    [PacketProperty]
    private long _keepAliveId;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadInt64(out _keepAliveId);
    }

}
