using Raspite.Tags;
using Raspite.Tags.Building;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record TranslatableComponent(
    string Translate,
    string? Fallback = null,
    List<Component>? With = null) : Component
{

    public override CompoundTagBuilder ToCompoundNbtBuilder(string name = "")
    {
        var builder = base.ToCompoundNbtBuilder(name)
            .AddString(Translate, "translate")
            .AddString(Fallback, "fallback");

        if (With is { Count: > 0 })
        {
            builder.Add(ListTag.Create(With.Select(with => with.ToNbt()), "with"));
        }

        return builder;
    }

}
