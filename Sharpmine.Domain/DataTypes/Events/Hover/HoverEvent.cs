using System.Text.Json.Serialization;

using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Events.Hover;

[JsonConverter(typeof(HoverEventConverter))]
public sealed record HoverEvent(string Action, IHoverEventContents Contents) : INbtSerializable
{

    public ITag ToNbt(string name = "")
    {
        return new CompoundTag([new StringTag(Action, "action"), Contents.ToNbt("contents")], name);
    }

}
