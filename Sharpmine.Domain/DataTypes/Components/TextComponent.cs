using Raspite.Tags;
using Raspite.Tags.Building;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record TextComponent(string Text) : Component
{

    public bool IsPlain => Style.IsEmpty
                           && ClickEvent is null
                           && HoverEvent is null
                           && !IsList;

    public override ITag ToNbt(string name = "") => (IsPlain)
        ? new StringTag(Text, name)
        : ToCompoundNbtBuilder(name).Build();

    public override CompoundTagBuilder ToCompoundNbtBuilder(string name = "")
    {
        return base.ToCompoundNbtBuilder(name)
            .AddString(Text, "text");
    }

    public static implicit operator TextComponent(string text) => new(text);

}
