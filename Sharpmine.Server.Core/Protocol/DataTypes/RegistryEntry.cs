using System.Buffers;

using Optional;

using Raspite;
using Raspite.Tags;

using Sharpmine.Server.Core.Protocol.Extensions;

namespace Sharpmine.Server.Core.Protocol.DataTypes;

public record RegistryEntry(string EntryId, Option<Tag> Data) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WritePrefixedOptional(Data, static (writer, data) => TagSerializer.Serialize(writer, data));
    }

}
