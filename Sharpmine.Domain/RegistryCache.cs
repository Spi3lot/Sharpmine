using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Sharpmine.Domain;

public class RegistryCache
{

    public RegistryCache(ImmutableArray<Registry> registries)
    {
        Registries = registries.ToFrozenDictionary(r => r.RegistryId, r => r);
    }

    public FrozenDictionary<string, Registry> Registries { get; }

}
