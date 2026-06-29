namespace Sharpmine.Domain.DataTypes;

public interface IBidirectionalDataType<TSelf> : IServerboundDataType<TSelf>, IClientboundDataType
    where TSelf : IBidirectionalDataType<TSelf>;
