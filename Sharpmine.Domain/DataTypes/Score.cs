using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes;

public readonly record struct Score(string Name, string Objective) : IConvertibleToNbt
{

    public ITag ToNbt(string name = "")
    {
        return new CompoundTag([new StringTag(Name, "name"), new StringTag(Objective, "objective")], name);
    }

}
