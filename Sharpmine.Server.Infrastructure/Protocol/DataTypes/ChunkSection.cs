using System.Buffers;

using Sharpmine.Domain.Tags;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public struct ChunkSection() : IClientboundDataType
{

    public short BlockCount { get; private set; }

    public PalettedContainer BlockStates { get; } = new(PalettedContainerType.BlockStates);

    public PalettedContainer Biomes { get; } = new(PalettedContainerType.Biomes);

    public int GetBlock(int x, int y, int z) => BlockStates.Get(x, y, z);

    public int SetBlock(int x, int y, int z, int stateId)
    {
        int oldStateId = BlockStates.Get(x, y, z);
        if (oldStateId == stateId) return oldStateId;

        BlockStates.Set(x, y, z, stateId);
        var airStates = BlockTags.GetStates(BlockTags.Air);
        bool wasAir = airStates.Contains(oldStateId);
        bool isAir = airStates.Contains(stateId);

        if (wasAir && !isAir) BlockCount++;
        else if (!wasAir && isAir) BlockCount--;

        return oldStateId;
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteInt16(BlockCount);
        BlockStates.Serialize(writer);
        Biomes.Serialize(writer);
    }

}
