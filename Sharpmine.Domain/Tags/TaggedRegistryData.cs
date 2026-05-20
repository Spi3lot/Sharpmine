namespace Sharpmine.Domain.Tags;

public readonly record struct TaggedRegistryData(string RegistryId, RegistryTagData[] Tags);
