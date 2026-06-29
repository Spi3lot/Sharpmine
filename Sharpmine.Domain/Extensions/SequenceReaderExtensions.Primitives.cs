using System.Buffers;
using System.Buffers.Binary;

namespace Sharpmine.Domain.Extensions;

public static partial class SequenceReaderExtensions
{

    extension(ref SequenceReader<byte> reader)
    {

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

    }

}
