using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Text.Json;

namespace Sharpmine.Domain.Blocks;

public class Block(
    Identifier id,
    Identifier type,
    string rawDefinitionJson,
    Dictionary<string, ImmutableArray<string>> possibleProperties,
    ImmutableArray<BlockState> states)
{

    public Identifier Id { get; } = id;

    public Identifier Type { get; } = type;

    public JsonElement Definition { get; } = JsonElement.Parse(rawDefinitionJson);

    public FrozenDictionary<string, ImmutableArray<string>> PossibleProperties { get; } = possibleProperties.ToFrozenDictionary();

    public ImmutableArray<BlockState> States { get; } = states;

    public BlockState DefaultState { get; } = states.SingleOrDefault(state => state.IsDefault) ?? states.First();

}
