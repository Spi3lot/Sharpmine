using Optional;

using Raspite.Tags;

namespace Sharpmine.Domain.Registries;

public readonly record struct RegistryEntry(string EntryId, Option<Tag> Data);
