using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes;

public interface IConvertibleToNbt
{

    ITag ToNbt(string name = "");

}
