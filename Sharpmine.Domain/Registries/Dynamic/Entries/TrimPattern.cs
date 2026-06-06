using Sharpmine.Domain.DataTypes.Components;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record TrimPattern(
    Identifier AssetId,
    Component Description,
    bool Decal);
