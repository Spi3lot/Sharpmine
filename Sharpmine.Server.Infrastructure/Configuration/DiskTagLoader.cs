using System.Collections.Immutable;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Sharpmine.Domain.Tags;
using Sharpmine.Server.Infrastructure.Protocol;

namespace Sharpmine.Server.Infrastructure.Configuration;

public class DiskTagLoader(RegistryProtocolIdMap registryProtocolIdMap, ILogger<DiskTagLoader> logger) : ITagLoader
{

    public ImmutableArray<TaggedRegistryData> Load()
    {
        string baseDir = AppContext.BaseDirectory;
        string registryTagsDir = Path.Combine(baseDir, "data", "minecraft", "tags");

        if (!Directory.Exists(registryTagsDir))
        {
            logger.LogWarning("Tags directory not found at {Path}. Skipping tag loading.", registryTagsDir);
            return [];
        }

        List<TaggedRegistryData> taggedRegistries = [];

        foreach (string registryId in registryProtocolIdMap.Map.Keys)
        {
            string cleanId = registryId.Replace("minecraft:", string.Empty);
            string targetFolder = cleanId.Replace('/', Path.DirectorySeparatorChar);
            string targetRegistryTagsDir = Path.Combine(registryTagsDir, targetFolder);

            if (!Directory.Exists(targetRegistryTagsDir))
            {
                logger.LogWarning("Tags subdirectory not found at {Path}. Skipping it.", targetRegistryTagsDir);
                continue;
            }

            List<RegistryTagData> tags = [];

            foreach (string file in Directory.EnumerateFiles(targetRegistryTagsDir, "*.json", SearchOption.AllDirectories))
            {
                string tagPath = Path.GetRelativePath(targetRegistryTagsDir, file);
                string tagName = "minecraft:" + tagPath.Replace(Path.DirectorySeparatorChar, '/').Replace(".json", string.Empty);

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
