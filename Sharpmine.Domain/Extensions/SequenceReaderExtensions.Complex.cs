using System.Buffers;
using System.Runtime.CompilerServices;

using Optional;

using Sharpmine.Domain.DataTypes;

namespace Sharpmine.Domain.Extensions;

public static partial class SequenceReaderExtensions
{

    extension(ref SequenceReader<byte> reader)
    {

        public bool TryRead<T>(out T value) where T : IServerboundDataType<T>
        {
            return T.TryDeserialize(ref reader, out value);
        }

        public bool TryReadPrefixedOptional<T>(out Option<T> result) where T : IServerboundDataType<T>
        {
            result = Option.None<T>();

            return reader.TryReadBoolean(out bool hasValue)
                   && reader.TryReadOptional(hasValue, out result);
        }

        public bool TryReadPrefixedOptional<T>(out Option<T> result, TryReadFunc<T> readFunc)
        {
            result = Option.None<T>();

            return reader.TryReadBoolean(out bool hasValue)
                   && reader.TryReadOptional(hasValue, out result, readFunc);
        }

        public bool TryReadOptional<T>(bool hasValue, out Option<T> result) where T : IServerboundDataType<T>
        {
            result = Option.None<T>();
            if (!hasValue) return true;
            if (!reader.TryRead(out T val)) return false;

            result = Option.Some(val);
            return true;
        }

        public bool TryReadOptional<T>(bool hasValue, out Option<T> result, TryReadFunc<T> readFunc)
        {
            result = Option.None<T>();
            if (!hasValue) return true;
            if (!readFunc(ref reader, out T val)) return false;

            result = Option.Some(val);
            return true;
        }

        public bool TryReadPrefixedArray<T>(out T[] result) where T : IServerboundDataType<T>
        {
            result = [];

            return reader.TryReadVarInt(out int length)
                   && reader.TryReadArray(length, out result);
        }

        public bool TryReadPrefixedArray<T>(out T[] result, TryReadFunc<T> readElementFunc)
        {
            result = [];

            return reader.TryReadVarInt(out int length)
                   && reader.TryReadArray(length, out result, readElementFunc);
        }

        public bool TryReadArray<T>(int length, out T[] result) where T : IServerboundDataType<T>
        {
            result = [];
            if (length < 0) return false;

            T[] array = new T[length];

            for (int i = 0; i < length; i++)
            {
                if (!reader.TryRead(out T element))
                {
                    return false;
                }

                array[i] = element;
            }

            result = array;
            return true;
        }

        public bool TryReadArray<T>(int length, out T[] result, TryReadFunc<T> readElementFunc)
        {
            result = [];
            if (length < 0) return false;

            T[] array = new T[length];

            for (int i = 0; i < length; i++)
            {
                if (!readElementFunc(ref reader, out T element))
                {
                    return false;
                }

                array[i] = element;
            }

            result = array;
            return true;
        }

        public bool TryReadEnum<TEnum>(out TEnum value)
            where TEnum : unmanaged, Enum
        {
            if (!reader.TryReadVarInt(out int rawValue))
            {
                value = default;
                return false;
            }

            value = Unsafe.As<int, TEnum>(ref rawValue);
            return true;
        }

        public bool TryReadEnum<TEnum>(out TEnum value, TryReadFunc<int> readFunc)
            where TEnum : unmanaged, Enum
        {
            if (!readFunc(ref reader, out int rawValue))
            {
                value = default;
                return false;
            }

            value = Unsafe.As<int, TEnum>(ref rawValue);
            return true;
        }

        public bool TryReadEnum<TEnum, TUnderlying>(out TEnum value, TryReadFunc<TUnderlying> readFunc)
            where TEnum : unmanaged, Enum
            where TUnderlying : unmanaged
        {
            if (!readFunc(ref reader, out TUnderlying rawValue))
            {
                value = default;
                return false;
            }

            value = Unsafe.As<TUnderlying, TEnum>(ref rawValue);
            return true;
        }

    }

}
