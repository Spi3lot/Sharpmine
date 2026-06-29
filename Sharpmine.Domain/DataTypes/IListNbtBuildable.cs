using Raspite.Tags;
using Raspite.Tags.Building;

namespace Sharpmine.Domain.DataTypes;

public interface IListNbtBuildable<TTag> where TTag : class, ITag
{

    ListTagBuilder<TTag> ToListNbtBuilder(string name = "");

}
