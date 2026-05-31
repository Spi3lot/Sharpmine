using Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public record TrimPattern(
    Identifier AssetId,
    Identifier TemplateItem,
    TrimDescription Description,
    bool Decal);
