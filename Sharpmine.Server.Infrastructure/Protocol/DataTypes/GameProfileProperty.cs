using System.Buffers;

using Optional;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public record GameProfileProperty(string Name, string Value, Option<string> Signature) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteString(Name);
        writer.WriteString(Value);
        writer.WritePrefixedOptional(Signature, static (writer, value) => writer.WriteString(value));
    }

}
