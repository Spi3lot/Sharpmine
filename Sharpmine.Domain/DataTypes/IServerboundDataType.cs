using System.Buffers;

namespace Sharpmine.Domain.DataTypes;

public interface IServerboundDataType<TSelf> where TSelf : IServerboundDataType<TSelf>
{

    static abstract bool TryDeserialize(ref SequenceReader<byte> reader, out TSelf value);

}
