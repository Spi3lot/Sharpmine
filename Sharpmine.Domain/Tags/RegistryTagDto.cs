using System.Collections.Immutable;

namespace Sharpmine.Domain.Tags;

public readonly record struct RegistryTagDto(Identifier TagName, ImmutableArray<Identifier> Values);
