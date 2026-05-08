using Optional;

using Sharpmine.Server.Protocol.DataTypes;

namespace Sharpmine.Server.Protocol.Extensions;

public static class BinaryWriterExtensions
{

    extension(BinaryWriter writer)
    {

        public void Write<T>(T value) where T : IClientboundDataType
        {
            value.Serialize(writer);
        }

        public void WritePrefixedOptional<T>(Option<T> value, Action<T> writeAction)
        {
            writer.Write(value.HasValue);
            writer.WriteOptional(value, writeAction);
        }

        public void WriteOptional<T>(Option<T> value, Action<T> writeAction)
        {
            value.MatchSome(writeAction);
        }

        public void WritePrefixedArray<T>(ReadOnlySpan<T> value, Action<T> writeElementAction)
        {
            writer.Write7BitEncodedInt(value.Length);
            writer.WriteArray(value, writeElementAction);
        }

        public void WriteArray<T>(ReadOnlySpan<T> value, Action<T> writeElementAction)
        {
            foreach (T element in value)
            {
                writeElementAction(element);
            }
        }

        public void WriteUuid(Guid uuid)
        {
            Span<byte> buffer = stackalloc byte[16];
            uuid.TryWriteBytes(buffer, bigEndian: true, out _);
            writer.Write(buffer);
        }

    }

}
