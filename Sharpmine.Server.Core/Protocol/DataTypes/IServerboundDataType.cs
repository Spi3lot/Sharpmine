using System.Buffers;

namespace Sharpmine.Server.Core.Protocol.DataTypes;

public interface IServerboundDataType<TSelf> where TSelf : IServerboundDataType<TSelf>
{

    static abstract bool TryDeserialize(ref SequenceReader<byte> reader, out TSelf value);

}
