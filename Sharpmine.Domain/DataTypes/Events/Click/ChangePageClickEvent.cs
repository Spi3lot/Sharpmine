using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Events.Click;

public sealed record ChangePageClickEvent(int Page) : ClickEvent("change_page")
{

    public override ITag ToNbt(string name = "")
    {
        return new CompoundTag([new StringTag(Action, "action"), new IntegerTag(Page, "page")], name);
    }

}
