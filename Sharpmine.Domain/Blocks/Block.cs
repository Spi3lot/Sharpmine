using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Text.Json;

namespace Sharpmine.Domain.Blocks;

public class Block
{

    public Block(Identifier id,
        Identifier type,
        string rawDefinitionJson,
        Dictionary<string, ImmutableArray<string>> possibleProperties,
        ImmutableArray<BlockState> states)
    {
        Id = id;
        Type = type;
        Definition = JsonElement.Parse(rawDefinitionJson);
        PossibleProperties = possibleProperties.ToFrozenDictionary();
        States = states;
        DefaultState = states.SingleOrDefault(state => state.IsDefault) ?? states.First();
        IsFluid = Type.Path is "liquid" or "bubble_column";
        IsSolid = DetermineSolidity();
    }

    public Identifier Id { get; }

    public Identifier Type { get; }

    public JsonElement Definition { get; }

    public FrozenDictionary<string, ImmutableArray<string>> PossibleProperties { get; }

    public ImmutableArray<BlockState> States { get; }

    public BlockState DefaultState { get; }

    public bool IsFluid { get; }

    public bool IsSolid { get; }

    private bool DetermineSolidity() => Type.Path is not (
        "air"
        or "liquid"
        or "bubble_column"
        or "tall_flower"

        // Flora
        or "flower"
        or "sapling"
        or "kelp"
        or "tall_seagrass"
        or "seagrass"
        or "sweet_berry_bush"
        or "dead_bush"
        or "glow_lichen"
        or "sculk_vein"

        // Decor
        or "vine"
        or "wall_torch"
        or "torch"
        or "wall_hanging_sign"
        or "hanging_sign"
        or "wall_sign"
        or "sign"
        or "wall_banner"
        or "banner"
        or "fire"
        or "cobweb"

        // Redstone
        or "tripwire_hook"
        or "tripwire"
        or "redstone_wire"
        or "lever"
        or "button");

}
