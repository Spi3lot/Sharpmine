using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;
using Sharpmine.Server.Infrastructure.Protocol.Attributes;
using Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Clientbound;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Serverbound;

public abstract partial record PingRequestPacket
{

    [PacketProperty]
    private long _timestamp;

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadInt64(out _timestamp);
    }

    public abstract PongResponsePacket CreatePongResponsePacket();

}
