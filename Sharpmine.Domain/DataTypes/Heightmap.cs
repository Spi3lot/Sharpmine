using System.Buffers;
using System.Collections.Frozen;

using Sharpmine.Domain.Extensions;
using Sharpmine.Domain.Tags;

namespace Sharpmine.Domain.DataTypes;

public readonly record struct Heightmap : IClientboundDataType
{

    private readonly BitStorage _bitStorage;

    public Heightmap(HeightmapType type, int dimensionHeight)
    {
        Type = type;
        _bitStorage = new BitStorage((int) Math.Ceiling(Math.Log2(dimensionHeight + 1)), 16 * 16);
    }

    public HeightmapType Type { get; }

    public int this[int x, int z]
    {
        get => _bitStorage[(z << 4) | x];
        set => _bitStorage[(z << 4) | x] = value;
    }

    public void Serialize(IBufferWriter<byte> writer)
    {
        writer.WriteVarInt((int) Type);
        writer.WritePrefixedArray(_bitStorage.Data, static (writer, @long) => writer.WriteInt64(@long));
    }

    public bool SatisfiesCriteria(int checkStateId) => Type switch
    {
        HeightmapType.WorldSurface => IsWorldSurface(checkStateId),
        HeightmapType.MotionBlocking => IsMotionBlocking(checkStateId),
        HeightmapType.MotionBlockingNoLeaves => IsMotionBlockingNoLeaves(checkStateId),
        _ => false
    };

    private static readonly RegistryTagDto MotionBlocking = new(
        "#sharpmine:motion_blocking",
        [
            .. Registries.Static.Blocks.All.Values
                .Where(b => b.IsFluid || b is { IsSolid: true, Type.Path: not ("bamboo_sapling" or "cactus") })
                .Select(b => b.Id)
        ]);

    private static readonly FrozenSet<int> AirStates = BlockTags.GetStates(BlockTags.Air);

    private static readonly FrozenSet<int> LeaveStates = BlockTags.GetStates(BlockTags.Leaves);

    private static readonly FrozenSet<int> MotionBlockingStates = BlockTags.GetStates(MotionBlocking);

    private static bool IsWorldSurface(int stateId) => !AirStates.Contains(stateId);

    private static bool IsMotionBlocking(int stateId) => MotionBlockingStates.Contains(stateId);

    private static bool IsMotionBlockingNoLeaves(int stateId) => MotionBlockingStates.Contains(stateId) && !LeaveStates.Contains(stateId);

}
