using Raspite.Tags.Building;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record SelectorComponent(string Selector, Component? Separator = null) : Component
{

    public override CompoundTagBuilder ToCompoundNbtBuilder(string name = "")
    {
        return base.ToCompoundNbtBuilder(name)
            .AddString(Selector, "selector")
            .Add(Separator?.ToNbt("separator"));
    }

}
