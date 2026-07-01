using Raspite.Tags.Building;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record KeybindComponent(string Keybind) : Component
{

    public override CompoundTagBuilder ToCompoundNbtBuilder(string name = "")
    {
        return base.ToCompoundNbtBuilder(name)
            .AddString(Keybind, "keybind");
    }

}
