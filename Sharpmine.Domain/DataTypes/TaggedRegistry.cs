using System.Buffers;

using Sharpmine.Domain.Extensions;

namespace Sharpmine.Domain.DataTypes;

public readonly record struct TaggedRegistry(Identifier RegistryId, RegistryTag[] Tags) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteIdentifier(RegistryId);
        writer.WritePrefixedArray(Tags);
    }

}
