using System.Collections.Immutable;

namespace Sharpmine.Domain.Tags;

public readonly record struct TaggedRegistryDto(Identifier RegistryId, ImmutableArray<RegistryTagDto> Tags);
