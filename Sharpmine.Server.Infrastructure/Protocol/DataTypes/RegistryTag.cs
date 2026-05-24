using System.Buffers;

using Sharpmine.Domain;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public readonly record struct RegistryTag(Identifier TagName, int[] Entries) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteIdentifier(TagName);
        writer.WritePrefixedArray(Entries, static (writer, entry) => writer.WriteVarInt(entry));
    }

}
