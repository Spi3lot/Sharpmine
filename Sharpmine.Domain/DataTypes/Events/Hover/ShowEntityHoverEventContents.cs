using Raspite.Tags;

using Sharpmine.Domain.DataTypes.Components;

namespace Sharpmine.Domain.DataTypes.Events.Hover;

public sealed record ShowEntityHoverEventContents(
    string Type,
    string Id,
    Component? Name = null) : IHoverEventContents
{

    public Tag ToNbt(string name = "")
    {
        List<Tag> tags = [new StringTag(Type, "type"), new StringTag(Id, "id")];
        if (Name is not null) tags.Add(Name.ToNbt("name"));
        return new CompoundTag([.. tags], name);
    }

}
