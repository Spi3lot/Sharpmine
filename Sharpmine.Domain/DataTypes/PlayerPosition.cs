using Raspite.Tags;
using Raspite.Tags.Building;

namespace Sharpmine.Domain.DataTypes;

public readonly record struct PlayerPosition(double X, double FeetY, double Z)
    : ICreatableFromNbt<PlayerPosition, ListTag<DoubleTag>>, IConvertibleToNbt<ListTag<DoubleTag>>
{

    public static PlayerPosition FromNbt(ListTag<DoubleTag> tag)
    {
        return new PlayerPosition(
            tag.GetDouble(0),
            tag.GetDouble(1),
            tag.GetDouble(2));
    }

    public ListTag<DoubleTag> ToNbt(string name = "")
    {
        return ListTagBuilder<DoubleTag>.Create(name)
            .AddDoubleRange(X, FeetY, Z)
            .Build();
    }
}
