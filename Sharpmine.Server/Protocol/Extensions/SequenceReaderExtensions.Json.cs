using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sharpmine.Server.Protocol.Extensions;

public static partial class SequenceReaderExtensions
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

    extension(ref SequenceReader<byte> reader)
    {

        public bool TryReadJsonString<T>(out T? result) where T : class
        {
            result = null;

            if (!reader.TryReadVarInt(out int length) || reader.Remaining < length)
            {
                return false;
            }

            var jsonSequence = reader.Sequence.Slice(reader.Position, length);
            reader.Advance(length);
            return TryParseJsonBytes(jsonSequence, out result, out _);
        }

        public bool TryReadJson<T>(out T? result) where T : class
        {
            if (TryParseJsonBytes(reader.UnreadSequence, out result, out long bytesConsumed))
            {
                reader.Advance(bytesConsumed);
                return true;
            }

            return false;
        }

    }

    private static bool TryParseJsonBytes<T>(
        ReadOnlySequence<byte> sequence,
        out T? result,
        out long bytesConsumed) where T : class
    {
        var utf8Reader = new Utf8JsonReader(sequence);

        try
        {
            result = JsonSerializer.Deserialize<T>(ref utf8Reader, Options);
            bytesConsumed = utf8Reader.BytesConsumed;
            return true;
        }
        catch (JsonException)
        {
            result = null;
            bytesConsumed = 0;
            return false;
        }
    }

}
