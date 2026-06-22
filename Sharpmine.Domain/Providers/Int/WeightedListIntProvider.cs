using System.Collections.Immutable;

namespace Sharpmine.Domain.Providers.Int;

public sealed record WeightedListIntProvider(ImmutableArray<WeightedDistributionEntry> Distribution) : IIntProvider
{

    public Identifier Type { get; } = Identifier.Minecraft("weighted_list");

    public int Sample(Random random)
    {
        if (Distribution.IsDefaultOrEmpty)
        {
            return 0;
        }

        int position = random.Next(Distribution.Sum(entry => entry.Weight));

        foreach (var entry in Distribution)
        {
            position -= entry.Weight;

            if (position < 0)
            {
                return entry.Data.Sample(random);
            }
        }

        throw new InvalidOperationException("Impossible.");
    }

}

public readonly record struct WeightedDistributionEntry(int Weight, IIntProvider Data);
