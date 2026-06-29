using System.Text.Json.Serialization;

using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Events.Click;

[JsonConverter(typeof(ClickEventConverter))]
public abstract record ClickEvent(string Action) : IConvertibleToNbt
{

    public abstract ITag ToNbt(string name = "");

}
