namespace Sharpmine.Domain.Providers.Int;

public sealed record ClampedNormalIntProvider(
    float Mean,
    float Deviation,
    int MinInclusive,
    int MaxInclusive) : IIntProvider
{

    public Identifier Type { get; } = Identifier.Minecraft("clamped_normal");

    public int Sample(Random random)
    {
        // Box-Muller transform
        double u1 = 1.0 - random.NextDouble(); // [0; 1) -> (0; 1]
        double u2 = 1.0 - random.NextDouble(); // [0; 1) -> (0; 1]
        double standardNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

        int result = (int) Math.Round(Mean + Deviation * standardNormal);
        return Math.Clamp(result, MinInclusive, MaxInclusive);
    }

}
