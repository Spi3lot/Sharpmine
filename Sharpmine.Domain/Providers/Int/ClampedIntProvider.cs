namespace Sharpmine.Domain.Providers.Int;

public sealed record ClampedIntProvider(IIntProvider Source, int MinInclusive, int MaxInclusive) : IIntProvider
{

    public Identifier Type { get; } = Identifier.Minecraft("clamped");

    public int Sample(Random random) => Math.Clamp(Source.Sample(random), MinInclusive, MaxInclusive);

}
