using System.Text.Json;

using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Events.Click;

public sealed record ShowDialogClickEvent(JsonElement Dialog) : ClickEvent("show_dialog")
{

    public override Tag ToNbt(string name = "")
    {
        List<Tag> tags = [new StringTag(Action, "action")];
        var dialogTag = JsonToNbtConverter.Convert(Dialog, "dialog");
        if (dialogTag is not null) tags.Add(dialogTag);
        return new CompoundTag([.. tags], name);
    }

}
