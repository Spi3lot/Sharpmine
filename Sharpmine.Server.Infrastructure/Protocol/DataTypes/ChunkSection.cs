using System.Buffers;
using System.Collections.Immutable;

using Sharpmine.Domain.Registries.Static;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public struct ChunkSection() : IClientboundDataType
{

    private static readonly ImmutableArray<int> AirStateIds =
    [
        .. Blocks.All.Values
            .Where(block => block.Type == BlockTypes.Air)
            .SelectMany(block => block.States)
            .Select(state => state.Id)
    ];

    public short BlockCount { get; private set; }

    public PalettedContainer BlockStates { get; } = new(PalettedContainerType.BlockStates);

    public PalettedContainer Biomes { get; } = new(PalettedContainerType.Biomes);

    public void SetBlock(int x, int y, int z, int stateId)
    {
        int oldStateId = BlockStates.Get(x, y, z);
        if (oldStateId == stateId) return;

        BlockStates.Set(x, y, z, stateId);
        bool wasAir = AirStateIds.Contains(oldStateId);
        bool isAir = AirStateIds.Contains(stateId);

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
