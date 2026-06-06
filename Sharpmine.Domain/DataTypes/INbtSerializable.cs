using Raspite.Tags;

namespace Sharpmine.Domain.DataTypes;

public interface INbtSerializable
{

    Tag ToNbt(string name = "");

}
