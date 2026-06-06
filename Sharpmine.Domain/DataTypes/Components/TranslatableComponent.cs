using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Components;

public sealed record TranslatableComponent(
    string Translate,
    string? Fallback = null,
    List<Component>? With = null) : Component
{

    public override Tag ToNbt(string name = "")
    {
        List<Tag> tags = [new StringTag(Translate, "translate")];

        if (Fallback is not null)
        {
            tags.Add(new StringTag(Fallback, "fallback"));
        }

        if (With is { Count: > 0 })
        {
            tags.Add(new ListTag([.. With.Select(with => with.ToNbt())], "with"));
        }

        ApplyStyleToNbt(tags);
        return new CompoundTag([.. tags]);
    }

}
