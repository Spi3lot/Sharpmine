using Raspite.Tags;
using Raspite.Tags.Building;

using Sharpmine.Domain.DataTypes.Nbt;

namespace Sharpmine.Domain.DataTypes;

public readonly record struct PlayerRotation(float Yaw, float Pitch)
    : ICreatableFromNbt<PlayerRotation, ListTag<FloatTag>>, IConvertibleToNbt<ListTag<FloatTag>>
{

    public static PlayerRotation FromNbt(ListTag<FloatTag> tag)
    {
        return new PlayerRotation(
            tag.GetFloat(0),
            tag.GetFloat(1));
    }

    public ListTag<FloatTag> ToNbt(string name = "")
    {
        return ListTagBuilder<FloatTag>.Create(name)
            .AddFloatRange(Yaw, Pitch)
            .Build();
    }

}
