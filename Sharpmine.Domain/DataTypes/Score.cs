using Raspite.Tags;

using Sharpmine.Domain.DataTypes.Nbt;

namespace Sharpmine.Domain.DataTypes;

public readonly record struct Score(string Name, string Objective) : IConvertibleToNbt<CompoundTag>
{

    public CompoundTag ToNbt(string name = "")
    {
        return new CompoundTag([new StringTag(Name, "name"), new StringTag(Objective, "objective")], name);
    }

}
