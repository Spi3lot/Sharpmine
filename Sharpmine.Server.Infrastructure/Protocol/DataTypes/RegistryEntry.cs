using System.Buffers;

using Optional;

using Raspite;
using Raspite.Tags;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public readonly record struct RegistryEntry(string EntryId, Option<Tag> Data) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteString(EntryId);
        writer.WritePrefixedOptional(Data, static (writer, data) => TagSerializer.Serialize(writer, data));
    }

}
