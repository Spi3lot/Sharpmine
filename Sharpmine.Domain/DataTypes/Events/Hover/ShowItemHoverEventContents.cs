using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Events.Hover;

public sealed record ShowItemHoverEventContents(string Id, int? Count = null, string? Tag = null) : IHoverEventContents
{
    public Tag ToNbt(string name = "")
    {
        List<Tag> tags = [new StringTag(Id, "id")];
        if (Count.HasValue) tags.Add(new IntegerTag(Count.Value, "count"));
        if (Tag is not null) tags.Add(new StringTag(Tag, "tag"));
        return new CompoundTag([.. tags], name);
    }
}