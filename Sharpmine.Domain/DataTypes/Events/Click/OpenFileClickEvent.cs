using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Events.Click;

public sealed record OpenFileClickEvent(string Path) : ClickEvent("open_file")
{

    public override CompoundTag ToNbt(string name = "")
    {
        return new CompoundTag([new StringTag(Action, "action"), new StringTag(Path, "path")], name);
    }

}
