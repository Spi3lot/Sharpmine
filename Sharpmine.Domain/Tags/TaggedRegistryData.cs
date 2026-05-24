namespace Sharpmine.Domain.Tags;

public readonly record struct TaggedRegistryData(Identifier RegistryId, RegistryTagData[] Tags);
