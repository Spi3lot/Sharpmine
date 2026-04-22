using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sharpmine.Server.Protocol.Extensions;

public static class StreamExtensions
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

    extension<T>(Stream stream) where T : class
    {

        public Task WriteJson(T value)
        {
            return JsonSerializer.SerializeAsync(stream, value, Options);
        }


        public T? ReadJson()
        {
            return JsonSerializer.Deserialize<T>(stream, Options);
        }

    }

}
