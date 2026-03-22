namespace Sharpmine.Server.Protocol.DataTypes;

public interface IProtocolDataType<out TSelf> where TSelf : IProtocolDataType<TSelf>
{

    static abstract TSelf Deserialize(BinaryReader reader);

    void Serialize(BinaryWriter writer);

}
