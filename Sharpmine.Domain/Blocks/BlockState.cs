using System.Collections.Frozen;

namespace Sharpmine.Domain.Blocks;

public class BlockState(int id, bool isDefault, Dictionary<string, string> properties)
{

    public int Id { get; init; } = id;

    public bool IsDefault { get; init; } = isDefault;

    public FrozenDictionary<string, string> Properties { get; init; } = properties.ToFrozenDictionary();

}
