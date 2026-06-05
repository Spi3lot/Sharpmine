namespace Sharpmine.Domain.Providers.Int;

public sealed record ConstantIntProvider(int Value) : IIntProvider
{

    public Identifier Type { get; } = Identifier.Minecraft("constant");

    public int Sample(Random random) => Value;

}
