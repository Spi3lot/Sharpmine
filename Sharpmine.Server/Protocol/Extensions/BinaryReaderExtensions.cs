using Optional;
using Sharpmine.Server.Protocol.DataTypes;

namespace Sharpmine.Server.Protocol.Extensions;

public static class BinaryReaderExtensions
{

    extension(BinaryReader reader)
    {

        public T Read<T>() where T : IProtocolDataType<T>
        {
            return T.Deserialize(reader);
        }

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

            for (int i = 0; i < length; i++)
            {
                array[i] = readElementAction();
            }

            return array;
        }

    }

}
