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

        public int WritePrefixedEnumerable<T>(IEnumerable<T> value) where T : IClientboundDataType
        {
            using var pooledWriter = new PooledByteBufferWriter();
            int count = pooledWriter.WriteEnumerable(value);
            writer.WriteVarInt(count);
            writer.Write(pooledWriter.WrittenSpan);
            return count;
        }

        public int WritePrefixedEnumerable<T>(IEnumerable<T> value, Action<IBufferWriter<byte>, T> writeElementAction)
        {
            using var pooledWriter = new PooledByteBufferWriter();
            int count = pooledWriter.WriteEnumerable(value, writeElementAction);
            writer.WriteVarInt(count);
            writer.Write(pooledWriter.WrittenSpan);
            return count;
        }

        public int WritePrefixedEnumerable<T, TState>(IEnumerable<T> value, TState state, Action<IBufferWriter<byte>, T, TState> writeElementAction)
        {
            using var pooledWriter = new PooledByteBufferWriter();
            int count = pooledWriter.WriteEnumerable(value, state, writeElementAction);
            writer.WriteVarInt(count);
            writer.Write(pooledWriter.WrittenSpan);
            return count;
        }

        public int WriteEnumerable<T>(IEnumerable<T> value) where T : IClientboundDataType
        {
            int count = 0;

            foreach (T element in value)
            {
                writer.Write(element);
                count++;
            }

            return count;
        }

        public int WriteEnumerable<T>(IEnumerable<T> value, Action<IBufferWriter<byte>, T> writeElementAction)
        {
            int count = 0;

            foreach (T element in value)
            {
                writeElementAction(writer, element);
                count++;
            }

            return count;
        }

        public int WriteEnumerable<T, TState>(IEnumerable<T> value, TState state, Action<IBufferWriter<byte>, T, TState> writeElementAction)
        {
            int count = 0;

            foreach (T element in value)
            {
                writeElementAction(writer, element, state);
                count++;
            }

            return count;
        }

        public void WritePrefixedSpan<T>(ReadOnlySpan<T> value) where T : IClientboundDataType
        {
            writer.WriteVarInt(value.Length);
            writer.WriteSpan(value);
        }

        public void WritePrefixedSpan<T>(ReadOnlySpan<T> value, Action<IBufferWriter<byte>, T> writeElementAction)
        {
            writer.WriteVarInt(value.Length);
            writer.WriteSpan(value, writeElementAction);
        }

        public void WritePrefixedSpan<T, TState>(ReadOnlySpan<T> value, TState state, Action<IBufferWriter<byte>, T, TState> writeElementAction)
        {
            writer.WriteVarInt(value.Length);
            writer.WriteSpan(value, state, writeElementAction);
        }

        public void WriteSpan<T>(ReadOnlySpan<T> value) where T : IClientboundDataType
        {
            foreach (T element in value)
            {
                writer.Write(element);
            }
        }

        public void WriteSpan<T>(ReadOnlySpan<T> value, Action<IBufferWriter<byte>, T> writeElementAction)
        {
            foreach (T element in value)
            {
                writeElementAction(writer, element);
            }
        }

        public void WriteSpan<T, TState>(ReadOnlySpan<T> value, TState state, Action<IBufferWriter<byte>, T, TState> writeElementAction)
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
