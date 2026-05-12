using System.Buffers;

using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Clientbound;

public abstract partial record PongResponsePacket
{

    public long Timestamp { get; init; }

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteInt64(Timestamp);
    }

}
