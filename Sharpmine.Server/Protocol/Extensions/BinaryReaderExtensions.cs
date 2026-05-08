using Optional;

using Sharpmine.Server.Protocol.DataTypes;

namespace Sharpmine.Server.Protocol.Extensions;

public static class BinaryReaderExtensions
{

    extension(BinaryReader reader)
    {

        public T Read<T>() where T : IServerboundDataType<T>
        {
            return T.Deserialize(reader);
        }

        // TODO: Replace (Prefixed)Optional(Array) with a custom IBidirectionalDataType
        //       to get rid of the delegate( allocation)s
        public Option<T> ReadPrefixedOptional<T>(Func<T> readAction)
        {
            return reader.ReadOptional(reader.ReadBoolean(), readAction);
        }

        public Option<T> ReadOptional<T>(bool hasValue, Func<T> readAction)
        {
            return (hasValue) ? Option.Some(readAction()) : Option.None<T>();
        }

        public T[] ReadPrefixedArray<T>(Func<T> readElementAction)
        {
            return reader.ReadArray(reader.Read7BitEncodedInt(), readElementAction);
        }

        public T[] ReadArray<T>(int length, Func<T> readElementAction)
        {
            T[] array = new T[length];
            reader.ReadIntoSpan(array, readElementAction);
            return array;
        }

        public void ReadIntoSpan<T>(Span<T> destination, Func<T> readElementAction)
        {
            for (int i = 0; i < destination.Length; i++)
            {
                destination[i] = readElementAction();
            }
        }

        public Guid ReadUuid()
        {
            Span<byte> buffer = stackalloc byte[16];
            reader.ReadExactly(buffer);
            return new Guid(buffer, bigEndian: true);
        }

    }

}
