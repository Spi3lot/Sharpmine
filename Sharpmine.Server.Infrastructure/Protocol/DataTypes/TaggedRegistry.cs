using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public readonly record struct TaggedRegistry(string RegistryId, RegistryTag[] Tags) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteString(RegistryId);
        writer.WritePrefixedArray(Tags);
    }

}
