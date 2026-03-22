using Sharpmine.Server.Protocol.DataTypes;
using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Login.Clientbound;

public record GameProfile(Guid Uuid, string Username, GameProfileProperty[] Properties) : IClientboundDataType
{

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Uuid.ToByteArray());
        writer.Write(Username);
        writer.WritePrefixedArray(Properties, writer.Write);
    }

}
