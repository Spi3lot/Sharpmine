namespace Sharpmine.Domain;

public readonly record struct Registry(string RegistryId, RegistryEntry[] Entries);
