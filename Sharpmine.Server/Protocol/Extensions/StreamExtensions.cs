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

        public Task WriteJsonAsync(T value, CancellationToken cancellationToken)
        {
            return JsonSerializer.SerializeAsync(stream, value, Options, cancellationToken);
        }


        public ValueTask<T?> ReadJsonAsync(CancellationToken cancellationToken)
        {
            return JsonSerializer.DeserializeAsync<T>(stream, Options, cancellationToken);
        }

    }

}
