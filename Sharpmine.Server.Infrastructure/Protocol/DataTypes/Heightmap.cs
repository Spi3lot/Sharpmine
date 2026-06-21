using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;


public readonly record struct Heightmap(HeightmapType Type) : IClientboundDataType
{

    private readonly long[] _data = [];

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteVarInt((int) Type);
        writer.WritePrefixedArray(_data, static (writer, @long) => writer.WriteInt64(@long));
    }

}