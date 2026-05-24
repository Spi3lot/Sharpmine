using Optional;

using Raspite.Tags;

namespace Sharpmine.Domain.Registries;

public readonly record struct RegistryEntry(Identifier EntryId, Option<Tag> Data);
