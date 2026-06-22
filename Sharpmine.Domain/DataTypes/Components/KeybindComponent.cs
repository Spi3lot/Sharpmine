using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record KeybindComponent(string Keybind) : Component
{

    public override ITag ToNbt(string name = "")
    {
        List<ITag> tags = [new StringTag(Keybind, "keybind")];
        ApplyStyleToNbt(tags);
        return new CompoundTag([.. tags], name);
    }

}
