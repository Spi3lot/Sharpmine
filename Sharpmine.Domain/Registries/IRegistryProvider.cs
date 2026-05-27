using System.Collections.Immutable;

namespace Sharpmine.Domain.Registries;

public interface IRegistryProvider : IProvider<ImmutableArray<RegistryDto>>;
