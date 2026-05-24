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

    public ImmutableArray<TaggedRegistryData> Get()
    {
        string baseDir = AppContext.BaseDirectory;
        string registryTagsDir = Path.Combine(baseDir, "data", "minecraft", "tags");

        if (!Directory.Exists(registryTagsDir))
        {
            logger.LogWarning("Tags directory not found at {Path}, skipping tag loading.", registryTagsDir);
            return [];
        }

        List<TaggedRegistryData> taggedRegistries = [];

        foreach (var registryId in protocolRegistryManifest.ProtocolIds.Keys)
        {
            string targetFolder = registryId.Path.Replace('/', Path.DirectorySeparatorChar);
            string targetRegistryTagsDir = Path.Combine(registryTagsDir, targetFolder);

            if (!Directory.Exists(targetRegistryTagsDir))
            {
                logger.LogTrace("Tags subdirectory not found at {Path}, skipping it.", targetRegistryTagsDir);
                continue;
            }

            List<RegistryTagData> tags = [];

            foreach (string file in Directory.EnumerateFiles(targetRegistryTagsDir, "*.json", SearchOption.AllDirectories))
            {
                string tagPath = Path.GetRelativePath(targetRegistryTagsDir, file);
                Identifier tagName = tagPath.Replace(Path.DirectorySeparatorChar, '/').Replace(".json", string.Empty);

                var jsonNode = JsonNode.Parse(File.ReadAllBytes(file))!.AsObject();
                var valuesArray = jsonNode["values"]?.AsArray();

                if (valuesArray == null)
                {
                    continue;
                }

                var tagValues = valuesArray.Select(valueNode => valueNode!.GetValue<string>())
                    .Where(entryName => !entryName.StartsWith('#'))
                    .Select(entryName => entryName.Contains(':') ? entryName : "minecraft:" + entryName)
                    .ToList();

                tags.Add(new RegistryTagData(tagName, [.. tagValues]));
            }

            taggedRegistries.Add(new TaggedRegistryData(registryId, [.. tags]));
        }

        return [.. taggedRegistries];
    }

}
