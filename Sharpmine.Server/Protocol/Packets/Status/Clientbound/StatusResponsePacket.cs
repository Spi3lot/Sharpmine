using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sharpmine.Server.Protocol.Packets.Status.Clientbound;

public partial record StatusResponsePacket(ServerStatus Status)
{

    public Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        // TODO: Don't create a new options object for every serialization
        var options = new JsonSerializerOptions
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

        return JsonSerializer.SerializeAsync(stream, Status, options, cancellationToken);
    }

}
