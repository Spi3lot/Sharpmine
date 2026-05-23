using System.Collections.Immutable;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Sharpmine.Domain.Registries;
using Sharpmine.Domain.Tags;

namespace Sharpmine.Server.Infrastructure.Configuration;

public class DiskTagLoader(RegistryCache registryCache, ILogger<DiskTagLoader> logger) : ITagLoader
{

    public ImmutableArray<TaggedRegistryData> Load()
    {
        string baseDir = AppContext.BaseDirectory;
        string registryTagsDir = Path.Combine(baseDir, "data", "minecraft", "tags");
        string registriesJsonPath = Path.Combine(baseDir, "generated", "reports", "registries.json");

        if (!Directory.Exists(registryTagsDir))
        {
            logger.LogWarning("Tags directory not found at {Path}. Tag loading skipped.", registryTagsDir);
            return [];
        }

        HashSet<string> knownRegistries = [.. registryCache.Registries.Keys];

        if (File.Exists(registriesJsonPath))
        {
            var rootNode = JsonNode.Parse(File.ReadAllBytes(registriesJsonPath))!.AsObject();

            foreach (var node in rootNode)
            {
                knownRegistries.Add(node.Key);
            }
        }

        List<TaggedRegistryData> taggedRegistries = [];

        foreach (string registryId in knownRegistries)
        {
            string cleanId = registryId.Replace("minecraft:", string.Empty);
            string targetFolder = cleanId.Replace('/', Path.DirectorySeparatorChar);
            string targetRegistryTagsDir = Path.Combine(registryTagsDir, targetFolder);

            if (!Directory.Exists(targetRegistryTagsDir))
            {
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
