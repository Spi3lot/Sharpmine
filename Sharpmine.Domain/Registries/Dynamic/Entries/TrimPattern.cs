using Sharpmine.Domain.DataTypes;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record TrimPattern(
    Identifier AssetId,
    TextComponent Description,
    bool Decal);
