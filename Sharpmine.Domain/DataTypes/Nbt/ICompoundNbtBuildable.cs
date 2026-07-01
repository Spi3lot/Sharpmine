using Raspite.Tags.Building;

namespace Sharpmine.Domain.DataTypes.Nbt;

public interface ICompoundNbtBuildable
{

    CompoundTagBuilder ToListNbtBuilder(string name = "");

}
