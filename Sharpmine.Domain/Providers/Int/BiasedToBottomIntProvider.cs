namespace Sharpmine.Domain.Providers.Int;

public sealed record BiasedToBottomIntProvider(int MinInclusive, int MaxInclusive) : IIntProvider
{

    public Identifier Type { get; } = Identifier.Minecraft("biased_to_bottom");

    public int Sample(Random random)
    {
        int range = MaxInclusive - MinInclusive + 1;
        return MinInclusive + random.Next(random.Next(range) + 1);
    }

}
