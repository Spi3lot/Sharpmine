using System.Buffers;

using Sharpmine.Domain.Blocks;
using Sharpmine.Domain.Extensions;
using Sharpmine.Domain.Tags;

namespace Sharpmine.Domain.DataTypes;

public struct ChunkSection(int defaultBiomeId) : IClientboundDataType
{

    public short BlockCount { get; private set; }

    public PalettedContainer BlockStates { get; } = new(PalettedContainerType.BlockStates, 0);

    public PalettedContainer Biomes { get; } = new(PalettedContainerType.Biomes, defaultBiomeId);

    public int GetBlock(int x, int y, int z) => BlockStates.Get(x, y, z);

    public void SetBlock(int x, int y, int z, BlockState state) => SetBlock(x, y, z, state.Id);

    public void SetBlock(int x, int y, int z, int stateId)
    {
        int oldStateId = GetBlock(x, y, z);
        if (oldStateId == stateId) return;

        BlockStates.Set(x, y, z, stateId);
        var airStates = BlockTags.GetStates(BlockTags.Air);
        bool wasAir = airStates.Contains(oldStateId);
        bool isAir = airStates.Contains(stateId);

        if (wasAir && !isAir) BlockCount++;
        else if (!wasAir && isAir) BlockCount--;
    }

    public int GetBiome(int x, int y, int z) => Biomes.Get(x / 4, y / 4, z / 4);

    public int SetBiome(int x, int y, int z, int biomeId)
    {
        int oldBiomeId = GetBiome(x, y, z);
        Biomes.Set(x / 4, y / 4, z / 4, biomeId);
        return oldBiomeId;
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteInt16(BlockCount);
        BlockStates.Serialize(writer);
        Biomes.Serialize(writer);
    }

}
