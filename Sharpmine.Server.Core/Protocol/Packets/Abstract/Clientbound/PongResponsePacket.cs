using System.Buffers;

using Sharpmine.Server.Core.Protocol.Extensions;

namespace Sharpmine.Server.Core.Protocol.Packets.Abstract.Clientbound;

public abstract partial record PongResponsePacket
{

    public long Timestamp { get; init; }

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteInt64(Timestamp);
    }

}
