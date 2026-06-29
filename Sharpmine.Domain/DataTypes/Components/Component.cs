using System.Text.Json.Serialization;

using Raspite.Tags;

using Sharpmine.Domain.DataTypes.Events.Click;
using Sharpmine.Domain.DataTypes.Events.Hover;

namespace Sharpmine.Domain.DataTypes.Components;

[JsonConverter(typeof(ComponentConverter))]
public abstract record Component : IConvertibleToNbt<ITag>
{

    public ComponentStyle Style { get; init; }

    public ClickEvent? ClickEvent { get; init; }

    public HoverEvent? HoverEvent { get; init; }

    public List<Component>? Extra { get; init; }

    public bool IsList => Extra is not null;

    public List<Component> AsList() => [this with { Extra = null }, .. Extra ?? []];

    public static Component? List(List<Component>? list) => (list?.Count > 0)
        ? list[0] with { Extra = [.. list[0].Extra ?? [], .. list[1..]] }
        : null;

    public static implicit operator Component(string text) => new TextComponent(text);

    public abstract ITag ToNbt(string name = "");

    protected void ApplyStyleToNbt(List<ITag> tags)
    {
        if (Style.Color is not null) tags.Add(new StringTag(Style.Color, "color"));
        if (Style.Font is not null) tags.Add(new StringTag(Style.Font, "font"));
        if (Style.Bold is not null) tags.Add(new ByteTag((byte) (Style.Bold.Value ? 1 : 0), "bold"));
        if (Style.Italic is not null) tags.Add(new ByteTag((byte) (Style.Italic.Value ? 1 : 0), "italic"));
        if (Style.Underlined is not null) tags.Add(new ByteTag((byte) (Style.Underlined.Value ? 1 : 0), "underlined"));
        if (Style.Strikethrough is not null) tags.Add(new ByteTag((byte) (Style.Strikethrough.Value ? 1 : 0), "strikethrough"));
        if (Style.Obfuscated is not null) tags.Add(new ByteTag((byte) (Style.Obfuscated.Value ? 1 : 0), "obfuscated"));
        if (Style.ShadowColor is not null) tags.Add(new IntegerTag(Style.ShadowColor.Value, "shadow_color"));
        if (Style.Insertion is not null) tags.Add(new StringTag(Style.Insertion, "insertion"));

        if (ClickEvent is not null) tags.Add(ClickEvent.ToNbt("click_event"));
        if (HoverEvent is not null) tags.Add(HoverEvent.ToNbt("hover_event"));

        if (Extra is { Count: > 0 })
        {
            tags.Add(ListTag.Create(Extra.Select(extra => extra.ToNbt()), "extra"));
        }
    }

}
