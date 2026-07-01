using Raspite.Tags.Building;

namespace Sharpmine.Domain.DataTypes.Nbt;

public interface ICompoundNbtBuildable
{

    CompoundTagBuilder ToCompoundNbtBuilder(string name = "");

}
