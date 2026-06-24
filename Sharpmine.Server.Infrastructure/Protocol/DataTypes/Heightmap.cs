using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;


public readonly record struct Heightmap(HeightmapType Type) : IClientboundDataType
{

    public long[] Data { get; init; }

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteVarInt((int) Type);
        writer.WritePrefixedArray(Data, static (writer, @long) => writer.WriteInt64(@long));
    }

}