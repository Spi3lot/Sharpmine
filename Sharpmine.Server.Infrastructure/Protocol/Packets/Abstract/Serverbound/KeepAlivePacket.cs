using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Attributes;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Serverbound;

public abstract partial record KeepAlivePacket
{

    [PacketProperty]
    private long _keepAliveId;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadInt64(out _keepAliveId);
    }

}
