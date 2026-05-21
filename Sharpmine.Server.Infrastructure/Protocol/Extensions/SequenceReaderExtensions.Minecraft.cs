using System.Buffers;
using System.Text;

namespace Sharpmine.Server.Infrastructure.Protocol.Extensions;

public static partial class SequenceReaderExtensions
{

    private const byte SegmentBits = 0x7F;

    private const byte ContinueBit = 0x80;

    public delegate bool TryReadFunc<T>(ref SequenceReader<byte> reader, out T value);

    extension(ref SequenceReader<byte> reader)
    {

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

    }

}
