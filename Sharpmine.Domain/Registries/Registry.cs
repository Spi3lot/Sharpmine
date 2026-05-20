namespace Sharpmine.Domain.Registries;

public readonly record struct Registry(string RegistryId, RegistryEntry[] Entries);
