using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes;

public interface IConvertibleToNbt<out TTag> where TTag : class, ITag
{

    TTag ToNbt(string name = "");

}
