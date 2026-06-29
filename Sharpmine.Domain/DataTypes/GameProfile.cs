using System.Buffers;

using Sharpmine.Domain.Extensions;

namespace Sharpmine.Domain.DataTypes;

public readonly record struct GameProfile(
    Guid Uuid,
    string Username,
    GameProfileProperty[] Properties) : IClientboundDataType
{

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteUuid(Uuid);
        writer.WriteString(Username);
        writer.WritePrefixedArray(Properties);
    }

}
