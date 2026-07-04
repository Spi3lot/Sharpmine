using System.Buffers;

using Sharpmine.Domain.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Clientbound;

public abstract partial record KeepAlivePacket
{

    public long KeepAliveId { get; init; }

    public bool Log => false;

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteInt64(KeepAliveId);
    }

}
