using System.Buffers;

using Sharpmine.Server.Core.Protocol.Extensions;

namespace Sharpmine.Server.Core.Protocol.DataTypes;

public record GameProfile(Guid Uuid, string Username, GameProfileProperty[] Properties) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteUuid(Uuid);
        writer.WriteString(Username);
        writer.WritePrefixedArray(Properties);
    }

}
