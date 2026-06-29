using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes;

public interface ICreatableFromNbt<out TSelf, in TTag>
    where TSelf : ICreatableFromNbt<TSelf, TTag>
    where TTag : ITag
{

    static abstract TSelf FromNbt(TTag tag);

}
