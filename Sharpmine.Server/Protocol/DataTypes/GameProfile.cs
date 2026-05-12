using System.Buffers;

using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.DataTypes;

public record GameProfile(Guid Uuid, string Username, GameProfileProperty[] Properties) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteUuid(Uuid);
        writer.WriteString(Username);
        writer.WritePrefixedArray(Properties, static (writer, property) => writer.Write(property));
    }

}
