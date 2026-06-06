using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record KeybindComponent(string Keybind) : Component
{

    public override Tag ToNbt(string name = "")
    {
        List<Tag> tags = [new StringTag(Keybind, "keybind")];
        ApplyStyleToNbt(tags);
        return new CompoundTag([.. tags], name);
    }

}
