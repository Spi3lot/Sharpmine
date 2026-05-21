using System.Buffers;
using System.Buffers.Binary;

namespace Sharpmine.Server.Infrastructure.Protocol.Extensions;

public static partial class BufferWriterExtensions
{

    extension(IBufferWriter<byte> writer)
    {

        public void WriteBoolean(bool value) => writer.WriteByte((value) ? (byte) 1 : (byte) 0);

        public void WriteByte(byte value)
        {
            var span = writer.GetSpan(1);
            span[0] = value;
            writer.Advance(1);
        }

        public void WriteSByte(sbyte value)
        {
            var span = writer.GetSpan(1);
            span[0] = unchecked((byte) value);
            writer.Advance(1);
        }

        public void WriteInt16(short value)
        {
            var span = writer.GetSpan(sizeof(short));
            BinaryPrimitives.WriteInt16BigEndian(span, value);
            writer.Advance(sizeof(short));
        }

        public void WriteUInt16(ushort value)
        {
            var span = writer.GetSpan(sizeof(ushort));
            BinaryPrimitives.WriteUInt16BigEndian(span, value);
            writer.Advance(sizeof(ushort));
        }

        public void WriteInt32(int value)
        {
            var span = writer.GetSpan(sizeof(int));
            BinaryPrimitives.WriteInt32BigEndian(span, value);
            writer.Advance(sizeof(int));
        }

        public void WriteUInt32(uint value)
        {
            var span = writer.GetSpan(sizeof(uint));
            BinaryPrimitives.WriteUInt32BigEndian(span, value);
            writer.Advance(sizeof(uint));
        }

        public void WriteInt64(long value)
        {
            var span = writer.GetSpan(sizeof(long));
            BinaryPrimitives.WriteInt64BigEndian(span, value);
            writer.Advance(sizeof(long));
        }

        public void WriteUInt64(ulong value)
        {
            var span = writer.GetSpan(sizeof(ulong));
            BinaryPrimitives.WriteUInt64BigEndian(span, value);
            writer.Advance(sizeof(ulong));
        }

        public void WriteSingle(float value) => writer.WriteInt32(BitConverter.SingleToInt32Bits(value));

        public void WriteDouble(double value) => writer.WriteInt64(BitConverter.DoubleToInt64Bits(value));

        public void WriteUuid(Guid value)
        {
            Span<byte> guidBytes = stackalloc byte[16];
            value.TryWriteBytes(guidBytes, bigEndian: true, out _);
            writer.Write(guidBytes);
        }

    }

}
