using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;

using Optional;

using Sharpmine.Server.Core.Protocol.DataTypes;

namespace Sharpmine.Server.Core.Protocol.Extensions;

public static partial class SequenceReaderExtensions
{

    public delegate bool TryReadFunc<T>(ref SequenceReader<byte> reader, out T value);

    private const byte SegmentBits = 0x7F;

    private const byte ContinueBit = 0x80;

    extension(ref SequenceReader<byte> reader)
    {

        #region Primitives

        public bool TryReadBoolean(out bool value)
        {
            value = false;
            if (!reader.TryRead(out byte b)) return false;

            value = b != 0;
            return true;
        }

        public bool TryReadByte(out byte value) => reader.TryRead(out value);

        public bool TryReadSByte(out sbyte value)
        {
            value = 0;
            if (!reader.TryRead(out byte unsignedValue)) return false;

            value = unchecked((sbyte) unsignedValue);
            return true;
        }

        public bool TryReadInt16(out short value) => reader.TryReadBigEndian(out value);

        public bool TryReadUInt16(out ushort value)
        {
            value = 0;
            if (!reader.TryReadBigEndian(out short signedValue)) return false;

            value = unchecked((ushort) signedValue);
            return true;
        }

        public bool TryReadInt32(out int value) => reader.TryReadBigEndian(out value);

        public bool TryReadUInt32(out uint value)
        {
            value = 0;
            if (!reader.TryReadBigEndian(out int signedValue)) return false;

            value = unchecked((uint) signedValue);
            return true;
        }

        public bool TryReadInt64(out long value) => reader.TryReadBigEndian(out value);

        public bool TryReadUInt64(out ulong value)
        {
            value = 0;
            if (!reader.TryReadBigEndian(out long signedValue)) return false;

            value = unchecked((ulong) signedValue);
            return true;
        }

        public bool TryReadSingle(out float value)
        {
            value = 0;
            if (!reader.TryReadBigEndian(out int intVal)) return false;

            value = BitConverter.Int32BitsToSingle(intVal);
            return true;
        }

        public bool TryReadDouble(out double value)
        {
            value = 0;
            if (!reader.TryReadBigEndian(out long longVal)) return false;

            value = BitConverter.Int64BitsToDouble(longVal);
            return true;
        }

        public bool TryReadUuid(out Guid value)
        {
            value = Guid.Empty;

            if (!reader.TryReadBigEndian(out long mostSig)
                || !reader.TryReadBigEndian(out long leastSig)) return false;

            Span<byte> guidBytes = stackalloc byte[16];
            BinaryPrimitives.WriteInt64BigEndian(guidBytes, mostSig);
            BinaryPrimitives.WriteInt64BigEndian(guidBytes[8..], leastSig);
            value = new Guid(guidBytes, bigEndian: true);
            return true;
        }

        #endregion

        #region Minecraft Specifics

        public bool TryReadVarInt(out int result) => reader.TryReadVarInt(out result, out _);

        public bool TryReadVarInt(out int result, out int size)
        {
            result = 0;
            size = 0;
            int position = 0;

            while (reader.TryRead(out byte currentByte))
            {
                size++;
                result |= (currentByte & SegmentBits) << position;
                if ((currentByte & ContinueBit) == 0) return true;

                position += 7;

                if (position >= 32)
                {
                    size = -1;
                    return false;
                }
            }

            return false;
        }

        public bool TryReadVarLong(out long result) => reader.TryReadVarLong(out result, out _);

        public bool TryReadVarLong(out long result, out int size)
        {
            result = 0;
            size = 0;
            int position = 0;

            while (reader.TryRead(out byte currentByte))
            {
                size++;
                result |= (long) (currentByte & SegmentBits) << position;
                if ((currentByte & ContinueBit) == 0) return true;

                position += 7;

                if (position >= 64)
                {
                    size = -1;
                    return false;
                }
            }

            return false;
        }

        public bool TryReadString(out string result, short maxLength = short.MaxValue)
        {
            result = string.Empty;

            if (!reader.TryReadVarInt(out int length)
                || length < 0
                || length > maxLength * 3
                || length > reader.Remaining) return false;

            var stringBytes = reader.Sequence.Slice(reader.Position, length);
            reader.Advance(length);

            result = (stringBytes.IsSingleSegment)
                ? Encoding.UTF8.GetString(stringBytes.FirstSpan)
                : Encoding.UTF8.GetString(stringBytes.ToArray());

            return true;
        }

        #endregion

        #region Complex Types

        public bool TryRead<T>(out T value) where T : IServerboundDataType<T>
        {
            return T.TryDeserialize(ref reader, out value);
        }

        public bool TryReadPrefixedOptional<T>(out Option<T> result, TryReadFunc<T> readFunc)
        {
            result = Option.None<T>();

            return reader.TryReadBoolean(out bool hasValue)
                   && reader.TryReadOptional(hasValue, out result, readFunc);
        }

        public bool TryReadOptional<T>(bool hasValue, out Option<T> result, TryReadFunc<T> readFunc)
        {
            result = Option.None<T>();
            if (!hasValue) return true;

            if (readFunc(ref reader, out T val))
            {
                result = Option.Some(val);
                return true;
            }

            return false;
        }

        public bool TryReadPrefixedArray<T>(TryReadFunc<T> readElementFunc, out T[] result)
        {
            result = [];

            return reader.TryReadVarInt(out int length)
                   && reader.TryReadArray(length, out result, readElementFunc);
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
            return reader.TryReadEnum(out value, static (ref reader, out value) => reader.TryReadVarInt(out value));
        }

        public bool TryReadEnum<TEnum>(out TEnum value, TryReadFunc<int> readFunc)
            where TEnum : unmanaged, Enum
        {
            return reader.TryReadEnum<TEnum, int>(out value, readFunc);
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

        #endregion

    }

}
