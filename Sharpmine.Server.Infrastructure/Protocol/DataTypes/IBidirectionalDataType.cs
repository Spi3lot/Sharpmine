namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public interface IBidirectionalDataType<TSelf> : IServerboundDataType<TSelf>, IClientboundDataType
    where TSelf : IBidirectionalDataType<TSelf>;
