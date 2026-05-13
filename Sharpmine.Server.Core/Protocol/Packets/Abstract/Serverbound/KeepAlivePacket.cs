using System.Buffers;

using Sharpmine.Server.Core.Protocol.Attributes;
using Sharpmine.Server.Core.Protocol.Extensions;

namespace Sharpmine.Server.Core.Protocol.Packets.Abstract.Serverbound;

public abstract partial record KeepAlivePacket
{

    [PacketProperty]
    private long _keepAliveId;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadInt64(out _keepAliveId);
    }

}
