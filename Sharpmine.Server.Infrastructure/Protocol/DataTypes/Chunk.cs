using System.Buffers;

using Raspite.Tags;

using Sharpmine.Domain;
using Sharpmine.Domain.Registries.Dynamic.Entries;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public sealed class Chunk : IClientboundDataType
{

    private readonly int _minY;

    private readonly Heightmap _worldSurfaceHeightmap;

    private readonly Heightmap _motionBlockingHeightmap;

    private readonly Heightmap _motionBlockingNoLeavesHeightmap;

    private readonly Dictionary<int, BlockEntity> _blockEntities = [];

    public Chunk(DimensionType dimension)
    {
        _minY = dimension.MinY;
        _worldSurfaceHeightmap = new Heightmap(HeightmapType.WorldSurface, dimension.Height);
        _motionBlockingHeightmap = new Heightmap(HeightmapType.MotionBlocking, dimension.Height);
        _motionBlockingNoLeavesHeightmap = new Heightmap(HeightmapType.MotionBlockingNoLeaves, dimension.Height);
        Sections = new ChunkSection[dimension.Height / 16];

        for (int i = 0; i < Sections.Length; i++)
        {
            Sections[i] = new ChunkSection();
        }
    }

    public ChunkSection[] Sections { get; }

    public Heightmap[] Heightmaps =>
    [
        _worldSurfaceHeightmap,
        _motionBlockingHeightmap,
        _motionBlockingNoLeavesHeightmap
    ];

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
        UpdateHeightmap(in _worldSurfaceHeightmap, x, y, z, stateId);
        UpdateHeightmap(in _motionBlockingHeightmap, x, y, z, stateId);
        UpdateHeightmap(in _motionBlockingNoLeavesHeightmap, x, y, z, stateId);
    }

    private void UpdateHeightmap(in Heightmap heightmap, int x, int y, int z, int stateId)
    {
        int height = y - _minY + 1;
        int currentPeak = heightmap[x, z];

        if (heightmap.SatisfiesCriteria(stateId))
        {
            if (height > currentPeak)
            {
                heightmap[x, z] = height;
            }
        }
        else if (height == currentPeak)
        {
            int newPeak = 0;

            for (int scanY = y - 1; scanY >= _minY; scanY--)
            {
                if (heightmap.SatisfiesCriteria(GetBlock(x, scanY, z)))
                {
                    newPeak = scanY - _minY + 1;
                    break;
                }
            }

            heightmap[x, z] = newPeak;
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
        using var pooledWriter = new PooledByteBufferWriter(131_072);
        pooledWriter.WriteArray(Sections);
        writer.WritePrefixed(pooledWriter.WrittenSpan);
    }

    private static int PackChunkPos(int x, int y, int z) => ((x & 15) << 20) | ((z & 15) << 16) | (y & 0xFFFF);

}
