using System.Buffers;
using System.Buffers.Binary;
using System.Text;

using Optional;
using Optional.Unsafe;

using Sharpmine.Server.Infrastructure.Protocol.DataTypes;

namespace Sharpmine.Server.Infrastructure.Protocol.Extensions;

public static partial class BufferWriterExtensions
{

    private const byte SegmentBits = 0x7F;

    private const byte ContinueBit = 0x80;

    extension(IBufferWriter<byte> writer)
    {

        #region Primitives

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

        #endregion

        #region Minecraft Specifics

        public void WriteVarInt(int value)
        {
            var span = writer.GetSpan(5); // 5 == ceil(32 / 7)
            int written = 0;

            while ((value & ~SegmentBits) != 0)
            {
                span[written++] = (byte) ((value & SegmentBits) | ContinueBit);
                value >>>= 7;
            }

            span[written++] = (byte) value;
            writer.Advance(written);
        }

        public void WriteVarLong(long value)
        {
            var span = writer.GetSpan(10); // 10 == ceil(64 / 7)
            int written = 0;

            while ((value & ~((long) SegmentBits)) != 0)
            {
                span[written++] = (byte) ((value & SegmentBits) | ContinueBit);
                value >>>= 7;
            }

            span[written++] = (byte) value;
            writer.Advance(written);
        }

        public void WriteString(string value)
        {
            int byteCount = Encoding.UTF8.GetByteCount(value);
            writer.WriteVarInt(byteCount);

            var span = writer.GetSpan(byteCount);
            Encoding.UTF8.GetBytes(value, span);
            writer.Advance(byteCount);
        }

        #endregion

        #region Complex Types

        public void Write<T>(T value) where T : IClientboundDataType
        {
            value.Serialize(writer);
        }

        public void WritePrefixedOptional<T>(Option<T> value) where T : IClientboundDataType
        {
            writer.WriteBoolean(value.HasValue);
            writer.WriteOptional(value);
        }

        public void WritePrefixedOptional<T>(Option<T> value, Action<IBufferWriter<byte>, T> writeAction)
        {
            writer.WriteBoolean(value.HasValue);
            writer.WriteOptional(value, writeAction);
        }

        public void WriteOptional<T>(Option<T> value) where T : IClientboundDataType
        {
            if (value.HasValue)
            {
                writer.Write(value.ValueOrFailure());
            }
        }

        public void WriteOptional<T>(Option<T> value, Action<IBufferWriter<byte>, T> writeAction)
        {
            if (value.HasValue)
            {
                writeAction(writer, value.ValueOrFailure());
            }
        }

        public void WritePrefixedArray<T>(ReadOnlySpan<T> value) where T : IClientboundDataType
        {
            writer.WriteVarInt(value.Length);
            writer.WriteArray(value);
        }

        public void WritePrefixedArray<T>(ReadOnlySpan<T> value, Action<IBufferWriter<byte>, T> writeElementAction)
        {
            writer.WriteVarInt(value.Length);
            writer.WriteArray(value, writeElementAction);
        }

        public void WriteArray<T>(ReadOnlySpan<T> value) where T : IClientboundDataType
        {
            foreach (T element in value)
            {
                writer.Write(element);
            }
        }

        public void WriteArray<T>(ReadOnlySpan<T> value, Action<IBufferWriter<byte>, T> writeElementAction)
        {
            foreach (T element in value)
            {
                writeElementAction(writer, element);
            }
        }

        #endregion

    }

}
