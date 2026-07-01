using System.Text.Json.Serialization;

using Raspite.Tags;
using Raspite.Tags.Building;

using Sharpmine.Domain.DataTypes.Events.Click;
using Sharpmine.Domain.DataTypes.Events.Hover;
using Sharpmine.Domain.DataTypes.Nbt;

namespace Sharpmine.Domain.DataTypes.Components;

[JsonConverter(typeof(ComponentConverter))]
public abstract record Component : IConvertibleToNbt<ITag>, ICompoundNbtBuildable
{

    private ComponentStyle _style;

    public ref ComponentStyle Style => ref _style;

    public ClickEvent? ClickEvent { get; init; }

    public HoverEvent? HoverEvent { get; init; }

    public List<Component>? Extra { get; init; }

    public bool IsList => Extra is { Count: > 0 };

    public List<Component> AsList() => [this with { Extra = null }, .. Extra ?? []];

    public static Component? List(List<Component>? list) => (list?.Count > 0)
        ? list[0] with { Extra = [.. list[0].Extra ?? [], .. list[1..]] }
        : null;

    public static implicit operator Component(string text) => new TextComponent(text);

    public virtual ITag ToNbt(string name = "") => ToCompoundNbtBuilder(name).Build();

    public virtual CompoundTagBuilder ToCompoundNbtBuilder(string name = "")
    {
        var builder = CompoundTagBuilder.Create(name)
            .AddString(_style.Color, "color")
            .AddString(_style.Font, "font")
            .AddBoolean(_style.Bold, "bold")
            .AddBoolean(_style.Italic, "italic")
            .AddBoolean(_style.Underlined, "underlined")
            .AddBoolean(_style.Strikethrough, "strikethrough")
            .AddBoolean(_style.Obfuscated, "obfuscated")
            .AddInteger(_style.ShadowColor, "shadow_color")
            .AddString(_style.Insertion, "insertion")
            .Add(ClickEvent?.ToNbt("click_event"))
            .Add(HoverEvent?.ToNbt("hover_event"));

        if (IsList)
        {
            builder.Add(ListTag.Create(Extra!.Select(extra => extra.ToNbt()), "extra"));
        }

        return builder;
    }

}
