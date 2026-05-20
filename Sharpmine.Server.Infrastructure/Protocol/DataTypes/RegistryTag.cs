using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public readonly record struct RegistryTag(string TagName, int[] Entries) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteString(TagName);
        writer.WritePrefixedArray(Entries, static (writer, entry) => writer.WriteVarInt(entry));
    }

}
