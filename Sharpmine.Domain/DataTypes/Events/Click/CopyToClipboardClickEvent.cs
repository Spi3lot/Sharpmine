using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Events.Click;

public sealed record CopyToClipboardClickEvent(string Value) : ClickEvent("copy_to_clipboard")
{

    public override ITag ToNbt(string name = "")
    {
        return new CompoundTag([new StringTag(Action, "action"), new StringTag(Value, "value")], name);
    }

}
