using Raspite.Tags;

using Sharpmine.Domain.DataTypes.Components;

namespace Sharpmine.Domain.DataTypes.Events.Hover;

public sealed record ShowTextHoverEventContents(Component Text) : IHoverEventContents
{

    public Tag ToNbt(string name = "") => Text.ToNbt(name);

}
