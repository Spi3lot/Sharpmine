namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record PaintingVariant(
    Identifier AssetId,
    int Width,
    int Height);
