using Optional;

using Raspite.Tags;

namespace Sharpmine.Domain.Registries;

public readonly record struct RegistryEntryDto(Identifier EntryId, Option<ITag> Data);
