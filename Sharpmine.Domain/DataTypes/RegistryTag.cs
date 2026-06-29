using System.Buffers;

using Sharpmine.Domain.Extensions;

namespace Sharpmine.Domain.DataTypes;

public readonly record struct RegistryTag(Identifier TagName, int[] Entries) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteIdentifier(TagName);
        writer.WritePrefixedArray(Entries, static (writer, entry) => writer.WriteVarInt(entry));
    }

}
