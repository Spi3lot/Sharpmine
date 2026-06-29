using System.Buffers;

using Optional;

using Raspite.Tags;

using Sharpmine.Domain.Extensions;

namespace Sharpmine.Domain.DataTypes;

public readonly record struct RegistryEntry(Identifier EntryId, Option<ITag> Data) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteIdentifier(EntryId);
        writer.WritePrefixedOptional(Data, static (writer, data) => writer.WriteNbt(data, network: true));
    }

}
