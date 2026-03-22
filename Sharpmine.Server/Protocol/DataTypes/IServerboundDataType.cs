namespace Sharpmine.Server.Protocol.DataTypes;

public interface IServerboundDataType<out TSelf> where TSelf : IServerboundDataType<TSelf>
{

    static abstract TSelf Deserialize(BinaryReader reader);

}
