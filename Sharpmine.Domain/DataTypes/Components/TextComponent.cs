using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record TextComponent(string Text) : Component
{

    public bool IsPlain => Style.IsEmpty
                           && ClickEvent is null
                           && HoverEvent is null
                           && !IsList;

    public override Tag ToNbt(string name = "")
    {
        if (IsPlain)
        {
            return new StringTag(Text, name);
        }

        List<Tag> tags = [new StringTag(Text, "text")];
        ApplyStyleToNbt(tags);
        return new CompoundTag([.. tags], name);
    }

    public static implicit operator TextComponent(string text) => new(text);

}
