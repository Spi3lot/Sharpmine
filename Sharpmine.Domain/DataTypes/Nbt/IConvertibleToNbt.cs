using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes.Nbt;

public interface IConvertibleToNbt<out TTag> where TTag : class, ITag
{

    TTag ToNbt(string name = "");

}
