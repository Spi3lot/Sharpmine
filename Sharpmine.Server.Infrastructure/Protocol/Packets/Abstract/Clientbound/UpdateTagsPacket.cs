using System.Buffers;

using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Clientbound;

public abstract partial record UpdateTagsPacket
{

    public TaggedRegistry[] TaggedRegistries { get; init; } = null!;

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WritePrefixedArray(TaggedRegistries);
    }

}
