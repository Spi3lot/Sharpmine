using System.Buffers;

using Sharpmine.Server.Core.Protocol.Attributes;
using Sharpmine.Server.Core.Protocol.Extensions;
using Sharpmine.Server.Core.Protocol.Packets.Abstract.Clientbound;

namespace Sharpmine.Server.Core.Protocol.Packets.Abstract.Serverbound;

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
