using System.Text.Json.Serialization;

using Raspite.Tags;

using Sharpmine.Domain.DataTypes.Nbt;

namespace Sharpmine.Domain.DataTypes.Events.Click;

[JsonConverter(typeof(ClickEventConverter))]
public abstract record ClickEvent(string Action) : IConvertibleToNbt<ITag>
{

    public abstract ITag ToNbt(string name = "");

}
