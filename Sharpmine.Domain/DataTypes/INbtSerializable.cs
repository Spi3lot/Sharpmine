using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes;

public interface INbtSerializable
{

    ITag ToNbt(string name = "");

}
