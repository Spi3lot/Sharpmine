using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Sharpmine.Domain.Blocks;

public class Block(
    Identifier id,
    Dictionary<string, ImmutableArray<string>> possibleProperties,
    ImmutableArray<BlockState> states)
{

    public Identifier Id { get; } = id;

    public FrozenDictionary<string, ImmutableArray<string>> PossibleProperties { get; } = possibleProperties.ToFrozenDictionary();

    public ImmutableArray<BlockState> States { get; } = states;

    public BlockState DefaultState { get; } = states.Single(state => state.IsDefault);

}
