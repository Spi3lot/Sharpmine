using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes;

public interface IConvertibleToNbt<out TTag> where TTag : ITag
{

    TTag ToNbt(string name = "");

}
