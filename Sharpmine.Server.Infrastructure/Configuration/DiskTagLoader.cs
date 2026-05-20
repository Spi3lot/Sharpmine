using System.Collections.Immutable;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Sharpmine.Domain.Tags;

namespace Sharpmine.Server.Infrastructure.Configuration;

public class DiskTagLoader(ILogger<DiskTagLoader> logger) : ITagLoader
{

    private static readonly Dictionary<string, string> FolderToRegistryId = new()
    {
        { "blocks", "minecraft:block" },
        { "items", "minecraft:item" },
        { "fluids", "minecraft:fluid" },
        { "entity_types", "minecraft:entity_type" },
        { "game_events", "minecraft:game_event" }
    };

    public ImmutableArray<TaggedRegistryData> Load()
    {
        string tagsDir = Path.Combine(AppContext.BaseDirectory, "data", "minecraft", "tags");

        if (!Directory.Exists(tagsDir))
        {
            logger.LogWarning("Tags directory not found at {Path}. Tag loading skipped.", tagsDir);
            return [];
        }

        List<TaggedRegistryData> taggedRegistries = [];

        foreach (string registryDir in Directory.EnumerateDirectories(tagsDir))
        {
            string registryFolder = Path.GetFileName(registryDir);

            string registryId = FolderToRegistryId.TryGetValue(registryFolder, out string? mappedId)
                ? mappedId
                : "minecraft:" + registryFolder;

            List<RegistryTagData> tags = [];

            foreach (string file in Directory.EnumerateFiles(registryDir, "*.json", SearchOption.AllDirectories))
            {
                string tagPath = Path.GetRelativePath(registryDir, file);
                string tagName = "minecraft:" + Path.GetFileNameWithoutExtension(tagPath).Replace(Path.DirectorySeparatorChar, '/');

                var jsonNode = JsonNode.Parse(File.ReadAllBytes(file))!.AsObject();
                var valuesArray = jsonNode["values"]?.AsArray();

                if (valuesArray == null)
                {
                    continue;
                }

                List<string> tagValues = valuesArray.Select(valueNode => valueNode!.GetValue<string>())
                    .Where(entryName => !entryName.StartsWith('#'))
                    .ToList();

                if (tagValues.Count > 0)
                {
                    tags.Add(new RegistryTagData(tagName, [.. tagValues]));
                }
            }

            if (tags.Count > 0)
            {
                taggedRegistries.Add(new TaggedRegistryData(registryId, [.. tags]));
            }
        }

        return [.. taggedRegistries];
    }

}
