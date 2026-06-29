using Raspite.Tags.Building;

namespace Sharpmine.Domain.DataTypes;

public interface ICompoundNbtBuildable
{

    CompoundTagBuilder ToListNbtBuilder(string name = "");

}
