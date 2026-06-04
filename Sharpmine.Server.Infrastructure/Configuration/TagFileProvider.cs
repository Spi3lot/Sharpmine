using System.Collections.Immutable;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Sharpmine.Domain;
using Sharpmine.Domain.Tags;
using Sharpmine.Server.Infrastructure.Protocol;

namespace Sharpmine.Server.Infrastructure.Configuration;

public class TagFileProvider(
    ProtocolRegistryManifest protocolRegistryManifest,
    ILogger<TagFileProvider> logger) : ITagProvider
{

    public ImmutableArray<TaggedRegistryDto> Get()
    {
        string baseDir = AppContext.BaseDirectory;
        string registryTagsDir = Path.Combine(baseDir, "data", "minecraft", "tags");

        if (!Directory.Exists(registryTagsDir))
        {
            logger.LogWarning("Tags directory not found at {Path}, skipping tag loading.", registryTagsDir);
            return [];
        }

        List<TaggedRegistryDto> taggedRegistries = [];

        foreach (var registryId in protocolRegistryManifest.ProtocolIds.Keys)
        {
            string targetFolder = registryId.Path.Replace('/', Path.DirectorySeparatorChar);
            string targetRegistryTagsDir = Path.Combine(registryTagsDir, targetFolder);

            if (!Directory.Exists(targetRegistryTagsDir))
            {
                logger.LogTrace("Tags subdirectory not found at {Path}, skipping it.", targetRegistryTagsDir);
                continue;
            }

            List<RegistryTagDto> tags = [];

            foreach (string file in Directory.EnumerateFiles(targetRegistryTagsDir, "*.json", SearchOption.AllDirectories))
            {
                string tagPath = Path.GetRelativePath(targetRegistryTagsDir, file);
                Identifier tagName = tagPath.Replace(Path.DirectorySeparatorChar, '/').Replace(".json", string.Empty);
                JsonObject? jsonObject;

                using (var fileStream = File.OpenRead(file))
                {
                    jsonObject = JsonNode.Parse(fileStream)!.AsObject();
                }

                var valuesArray = jsonObject["values"]?.AsArray();

                if (valuesArray is null)
                {
                    continue;
                }

                var tagValues = valuesArray.Select(valueNode => new Identifier(valueNode!.GetValue<string>()));
                tags.Add(new RegistryTagDto(tagName, [.. tagValues]));
            }

            taggedRegistries.Add(new TaggedRegistryDto(registryId, [.. tags]));
        }

        return [.. taggedRegistries];
    }

}
