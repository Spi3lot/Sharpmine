using System.Buffers;

using Sharpmine.Server.Protocol.Attributes;
using Sharpmine.Server.Protocol.Extensions;
using Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

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
