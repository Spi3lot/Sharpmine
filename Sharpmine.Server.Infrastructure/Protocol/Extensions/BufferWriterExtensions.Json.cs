using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

using Sharpmine.Domain;

namespace Sharpmine.Server.Infrastructure.Protocol.Extensions;

public static partial class BufferWriterExtensions
{

    private static readonly JsonSerializerOptions Options = new()
    {
        AllowDuplicateProperties = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
                         | JsonNumberHandling.AllowNamedFloatingPointLiterals,
    };

    extension(IBufferWriter<byte> writer)
    {

        public void WriteJsonString<T>(T value)
        {
            using var pooledWriter = new PooledByteBufferWriter(1024);
            pooledWriter.WriteJson(value);
            writer.WriteVarInt(pooledWriter.WrittenCount);
            writer.Write(pooledWriter.WrittenSpan);
        }

        public void WriteJson<T>(T value) => writer.WriteJson(value, Options);

        public void WriteJson<T>(T value, JsonSerializerOptions options)
        {
            using var jsonWriter = new Utf8JsonWriter(writer);
            JsonSerializer.Serialize(jsonWriter, value, options);
        }

    }

}
