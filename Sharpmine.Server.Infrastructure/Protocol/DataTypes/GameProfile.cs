using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public record GameProfile(Guid Uuid, string Username, GameProfileProperty[] Properties) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteUuid(Uuid);
        writer.WriteString(Username);
        writer.WritePrefixedArray(Properties);
    }

}
