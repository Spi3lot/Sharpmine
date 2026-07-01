using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Nbt;

public interface ICreatableFromNbt<out TSelf, in TTag>
    where TSelf : ICreatableFromNbt<TSelf, TTag>
    where TTag : class, ITag
{

    static abstract TSelf FromNbt(TTag tag);

}
