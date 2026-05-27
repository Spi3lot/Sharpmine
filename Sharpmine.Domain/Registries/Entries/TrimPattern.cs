using Sharpmine.Domain.Registries.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Entries;

public record TrimPattern(
    Identifier AssetId,
    Identifier TemplateItem,
    TrimDescription Description,
    bool Decal);
