using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Events.Click;

public sealed record SuggestCommandClickEvent(string Command) : ClickEvent("suggest_command")
{

    public override CompoundTag ToNbt(string name = "")
    {
        return new CompoundTag([new StringTag(Action, "action"), new StringTag(Command, "command")], name);
    }

}
