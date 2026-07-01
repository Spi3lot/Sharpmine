using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Nbt;

public interface ILoadableFromNbt<in TTag> where TTag : class, ITag
{

    void LoadFromNbt(TTag tag);

}
