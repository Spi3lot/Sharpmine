using System.Buffers;
using System.Collections.Frozen;

using Sharpmine.Domain.Registries.Static;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public struct ChunkSection() : IClientboundDataType
{

    private static readonly FrozenSet<int> AirStateIds = Blocks.All.Values
        .Where(block => block.Type == BlockTypes.Air)
        .Select(state => state.DefaultState.Id)
        .ToFrozenSet();

    public short BlockCount { get; private set; }

    public PalettedContainer BlockStates { get; } = new(PalettedContainerType.BlockStates);

    public PalettedContainer Biomes { get; } = new(PalettedContainerType.Biomes);

    public int GetBlock(int x, int y, int z) => BlockStates.Get(x, y, z);

    public void SetBlock(int x, int y, int z, int stateId)
    {
        int oldStateId = BlockStates.Get(x, y, z);
        if (oldStateId == stateId) return;

        BlockStates.Set(x, y, z, stateId);
        var airStates = BlockTags.GetStates(BlockTags.Air);
        bool wasAir = airStates.Contains(oldStateId);
        bool isAir = airStates.Contains(stateId);

        if (wasAir && !isAir) BlockCount++;
        else if (!wasAir && isAir) BlockCount--;
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteInt16(BlockCount);
        BlockStates.Serialize(writer);
        Biomes.Serialize(writer);
    }

}
