using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Sharpmine.Domain.Blocks;

public class Block(
    Identifier id,
    ImmutableArray<BlockState> states,
    Dictionary<string, ImmutableArray<string>> possibleProperties)
{

    public Identifier Id { get; } = id;

    public ImmutableArray<BlockState> States { get; } = states;

    public BlockState DefaultState { get; } = states.Single(state => state.IsDefault);

    public FrozenDictionary<string, ImmutableArray<string>> PossibleProperties { get; } = possibleProperties.ToFrozenDictionary();

}
