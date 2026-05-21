using System.Buffers;
using System.Text;

using Raspite;
using Raspite.Tags;

namespace Sharpmine.Server.Infrastructure.Protocol.Extensions;

public static partial class BufferWriterExtensions
{

    private const byte SegmentBits = 0x7F;

    private const byte ContinueBit = 0x80;

    [ThreadStatic]
    private static readonly ArrayBufferWriter<byte> ArrayBufferWriter;

    static BufferWriterExtensions()
    {
        ArrayBufferWriter = new ArrayBufferWriter<byte>();
    }

    extension(IBufferWriter<byte> writer)
    {

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

        public void WriteNbt(Tag value, bool network = true)
        {
            if (!network)
            {
                TagSerializer.Serialize(writer, value);
                return;
            }

            ArrayBufferWriter.Clear();
            TagSerializer.Serialize(ArrayBufferWriter, value);
            var span = ArrayBufferWriter.WrittenSpan;
            writer.Write(span[..1]);
            writer.Write(span[3..]);
        }

    }

}
