namespace Sharpmine.Domain.Registries;

public readonly record struct Registry(Identifier RegistryId, RegistryEntry[] Entries);
