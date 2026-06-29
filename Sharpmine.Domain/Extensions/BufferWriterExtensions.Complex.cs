using System.Buffers;

using Optional;
using Optional.Unsafe;

using Sharpmine.Domain.DataTypes;

namespace Sharpmine.Domain.Extensions;

public static partial class BufferWriterExtensions
{

    extension(IBufferWriter<byte> writer)
    {

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

        public void WritePrefixedOptional<T, TState>(Option<T> value, TState state, Action<IBufferWriter<byte>, T, TState> writeAction)
        {
            writer.WriteBoolean(value.HasValue);
            writer.WriteOptional(value, state, writeAction);
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

        public void WriteOptional<T, TState>(Option<T> value, TState state, Action<IBufferWriter<byte>, T, TState> writeAction)
        {
            if (value.HasValue)
            {
                writeAction(writer, value.ValueOrFailure(), state);
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

        public void WritePrefixedArray<T, TState>(ReadOnlySpan<T> value, TState state, Action<IBufferWriter<byte>, T, TState> writeElementAction)
        {
            writer.WriteVarInt(value.Length);
            writer.WriteArray(value, state, writeElementAction);
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

        public void WriteArray<T, TState>(ReadOnlySpan<T> value, TState state, Action<IBufferWriter<byte>, T, TState> writeElementAction)
        {
            foreach (T element in value)
            {
                writeElementAction(writer, element, state);
            }
        }

        public void WritePrefixed(ReadOnlySpan<byte> value)
        {
            writer.WriteVarInt(value.Length);
            writer.Write(value);
        }

    }

}
