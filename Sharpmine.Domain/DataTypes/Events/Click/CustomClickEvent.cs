using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Events.Click;

public sealed record CustomClickEvent(Identifier Id, string? Payload = null) : ClickEvent("custom")
{

    public override Tag ToNbt(string name = "")
    {
        List<Tag> tags = [new StringTag(Action, "action"), new StringTag(Id, "id")];
        if (Payload is not null) tags.Add(new StringTag(Payload, "payload"));
        return new CompoundTag([.. tags], name);
    }

}
