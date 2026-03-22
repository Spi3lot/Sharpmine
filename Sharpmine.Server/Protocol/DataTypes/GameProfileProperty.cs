using Optional;

using Sharpmine.Server.Protocol.DataTypes;
using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Login.Clientbound;

public record GameProfileProperty(string Name, string Value, Option<string> Signature) : IClientboundDataType
{

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Name);
        writer.Write(Value);
        writer.WritePrefixedOptional(Signature, writer.Write);
    }

}
