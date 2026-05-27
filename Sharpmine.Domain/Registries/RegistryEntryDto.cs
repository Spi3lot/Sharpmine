using Optional;

using Raspite.Tags;

namespace Sharpmine.Domain.Registries;

public readonly record struct RegistryEntryDto(Identifier EntryId, Option<Tag> Data);
