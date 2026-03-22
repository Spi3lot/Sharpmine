namespace Sharpmine.Server.Protocol.DataTypes;

public interface IClientboundDataType
{

    void Serialize(BinaryWriter writer);

}