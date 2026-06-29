using System.Buffers;

using Optional;

using Sharpmine.Domain.Extensions;

namespace Sharpmine.Domain.DataTypes;

public readonly record struct GameProfileProperty(
    string Name,
    string Value,
    Option<string> Signature) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteString(Name);
        writer.WriteString(Value);
        writer.WritePrefixedOptional(Signature, static (writer, signature) => writer.WriteString(signature));

    }

}
