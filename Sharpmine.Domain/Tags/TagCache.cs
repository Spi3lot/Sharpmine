using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Sharpmine.Domain.Tags;

public class TagCache
{

    public TagCache(ImmutableArray<TaggedRegistryData> registries)
    {
        Registries = registries.ToFrozenDictionary(r => r.RegistryId, r => r);
    }

    public FrozenDictionary<string, TaggedRegistryData> Registries { get; }

}
