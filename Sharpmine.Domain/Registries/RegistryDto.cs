using System.Collections.Immutable;

namespace Sharpmine.Domain.Registries;

public readonly record struct RegistryDto(Identifier RegistryId, ImmutableArray<RegistryEntryDto> Entries);
