using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record SelectorComponent(string Selector, Component? Separator = null) : Component
{

    public override Tag ToNbt(string name = "")
    {
        List<Tag> tags = [new StringTag(Selector, "selector")];

        if (Separator is not null)
        {
            tags.Add(Separator.ToNbt("separator"));
        }

        ApplyStyleToNbt(tags);
        return new CompoundTag([.. tags], name);
    }

}
