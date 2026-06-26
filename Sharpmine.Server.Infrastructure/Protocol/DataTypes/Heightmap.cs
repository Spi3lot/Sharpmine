using System.Buffers;

using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public readonly record struct Heightmap : IClientboundDataType
{

    private readonly BitStorage _bitStorage;

    public Heightmap(HeightmapType type, int dimensionHeight)
    {
        Type = type;
        _bitStorage = new BitStorage((int) Math.Ceiling(Math.Log2(dimensionHeight + 1)), 16 * 16);
    }

    public HeightmapType Type { get; }

    public int this[int index]
    {
        get => _bitStorage[index];
        set => _bitStorage[index] = value;
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteVarInt((int) Type);
        writer.WritePrefixedArray(_bitStorage.Data, static (writer, @long) => writer.WriteInt64(@long));
    }

}
