using System.Collections.Immutable;

namespace Sharpmine.Domain.Registries;

public interface IRegistryLoader
{

    ImmutableArray<Registry> Load();

}
