using System.Collections.Immutable;

namespace Sharpmine.Domain;

public interface IRegistryLoader
{

    ImmutableArray<Registry> Load();

}
