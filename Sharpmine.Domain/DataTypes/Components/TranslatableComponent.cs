using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record TranslatableComponent(
    string Translate,
    string? Fallback = null,
    List<Component>? With = null) : Component
{

    public override ITag ToNbt(string name = "")
    {
        List<ITag> tags = [new StringTag(Translate, "translate")];

        if (Fallback is not null)
        {
            tags.Add(new StringTag(Fallback, "fallback"));
        }

        if (With is { Count: > 0 })
        {
            tags.Add(ListTag.Create(With.Select(with => with.ToNbt()), "with"));
        }

        ApplyStyleToNbt(tags);
        return new CompoundTag([.. tags]);
    }

}
