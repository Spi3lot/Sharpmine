using Optional;
using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.DataTypes;

public record GameProfileProperty(string Name, string Value, Option<string> Signature) : IClientboundDataType
{

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Name);
        writer.Write(Value);
        writer.WritePrefixedOptional(Signature, writer.Write);
    }

}
