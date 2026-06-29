using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes;

public interface ILoadableFromNbt<in TTag> where TTag : ITag
{

    void LoadFromNbt(TTag tag);

}
