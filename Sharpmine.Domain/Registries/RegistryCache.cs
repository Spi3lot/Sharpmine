using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Sharpmine.Domain.Registries;

public class RegistryCache
{

    public RegistryCache(ImmutableArray<RegistryDto> registries)
    {
        Registries = registries.ToFrozenDictionary(r => r.RegistryId, r => r);
    }

    public FrozenDictionary<Identifier, RegistryDto> Registries { get; }

}
