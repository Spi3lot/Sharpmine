using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Events.Click;

public sealed record OpenUrlClickEvent(string Url) : ClickEvent("open_url")
{

    public override Tag ToNbt(string name = "")
    {
        return new CompoundTag([new StringTag(Action, "action"), new StringTag(Url, "url")], name);
    }

}
