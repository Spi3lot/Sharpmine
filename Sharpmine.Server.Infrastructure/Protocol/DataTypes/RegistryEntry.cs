using System.Buffers;

using Optional;

using Raspite.Tags;

using Sharpmine.Domain;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public readonly record struct RegistryEntry(Identifier EntryId, Option<Tag> Data) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteIdentifier(EntryId);
        writer.WritePrefixedOptional(Data, static (writer, data) => writer.WriteNbt(data, network: true));
    }

}
