namespace Sharpmine.Server.Protocol.DataTypes;

public interface IBidirectionalDataType<out TSelf> : IServerboundDataType<TSelf>, IClientboundDataType
    where TSelf : IBidirectionalDataType<TSelf>;
