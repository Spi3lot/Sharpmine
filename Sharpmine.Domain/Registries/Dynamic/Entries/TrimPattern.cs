using Sharpmine.Domain.DataTypes;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public record TrimPattern(
    Identifier AssetId,
    TextComponent Description,
    bool Decal);
