using System.Buffers;

using Sharpmine.Domain;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public readonly record struct TaggedRegistry(Identifier RegistryId, RegistryTag[] Tags) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteIdentifier(RegistryId);
        writer.WritePrefixedArray(Tags);
    }

}
