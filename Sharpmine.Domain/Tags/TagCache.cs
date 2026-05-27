using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Sharpmine.Domain.Tags;

public class TagCache
{

    public TagCache(ImmutableArray<TaggedRegistryDto> registries)
    {
        TagsByRegistry = registries.ToFrozenDictionary(r => r.RegistryId, r => r);
    }

    public FrozenDictionary<Identifier, TaggedRegistryDto> TagsByRegistry { get; }

}
