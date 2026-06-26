using System.Buffers;

using Raspite.Tags;

using Sharpmine.Domain.Registries.Dynamic.Entries;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public sealed class Chunk : IClientboundDataType
{

    [ThreadStatic]
    private static ArrayBufferWriter<byte>? _chunkDataWriter;

    private readonly Heightmap _motionBlockingHeightmap;

    private readonly int _minY;

    private readonly Dictionary<int, BlockEntity> _blockEntities = [];

    public Chunk(DimensionType dimension)
    {
        _minY = dimension.MinY;
        _motionBlockingHeightmap = new Heightmap(HeightmapType.MotionBlocking, dimension.Height);
        Sections = new ChunkSection[dimension.Height / 16];

        for (int i = 0; i < Sections.Length; i++)
        {
            Sections[i] = new ChunkSection();
        }
    }

    public ChunkSection[] Sections { get; }

    public Heightmap[] Heightmaps => [_motionBlockingHeightmap];

    public BlockEntity[] BlockEntities =>
    [
        .. _blockEntities.Values.Select(blockEntity => blockEntity with
        {
            Data = blockEntity.Data.ToBuilder()
                .Remove("x")
                .Remove("y")
                .Remove("z")
                .Build()
        })
    ];

    public ref ChunkSection GetSection(int blockY) => ref Sections[(blockY - _minY) / 16];

    public int GetBlock(int x, int y, int z) => GetSection(y).GetBlock(x, y & 15, z);

    public void SetBlock(int x, int y, int z, int stateId)
    {
        GetSection(y).SetBlock(x, y & 15, z, stateId);
        int heightmapIndex = (z * 16) + x;
        int heightValue = y - _minY + 1;

        // TODO: Check if block is air
        if (_motionBlockingHeightmap[heightmapIndex] < heightValue)
        {
            _motionBlockingHeightmap[heightmapIndex] = heightValue;
        }
    }

    public BlockEntity GetBlockEntity(int x, int y, int z)
    {
        return _blockEntities[PackChunkPos(x, y, z)];
    }

    public void SetBlockEntity(int x, int y, int z, int typeId, CompoundTag data)
    {
        _blockEntities[PackChunkPos(x, y, z)] = new BlockEntity(x, y, z, typeId, data);
    }

    public bool RemoveBlockEntity(int x, int y, int z)
    {
        return _blockEntities.Remove(PackChunkPos(x, y, z));
    }

    public bool RemoveBlockEntity(int x, int y, int z, out BlockEntity blockEntity)
    {
        return _blockEntities.Remove(PackChunkPos(x, y, z), out blockEntity);
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        _chunkDataWriter ??= new ArrayBufferWriter<byte>(131_072);
        _chunkDataWriter.Clear();
        _chunkDataWriter.WriteArray(Sections);
        writer.WritePrefixed(_chunkDataWriter.WrittenSpan);
    }

    private static int PackChunkPos(int x, int y, int z) => ((x & 15) << 20) | ((z & 15) << 16) | (y & 0xFFFF);

}
