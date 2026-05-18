using Optional;

using Raspite.Tags;

namespace Sharpmine.Domain;

public readonly record struct RegistryEntry(string EntryId, Option<Tag> Data);
