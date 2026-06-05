namespace Sharpmine.Domain.Providers.Int;

public sealed record UniformIntProvider(int MinInclusive, int MaxInclusive) : IIntProvider
{

    public Identifier Type { get; } = Identifier.Minecraft("uniform");

    public int Sample(Random random) => random.Next(MinInclusive, MaxInclusive + 1);

}
