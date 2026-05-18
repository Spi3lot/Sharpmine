using System.Collections.Immutable;
using System.Text.Json.Nodes;

using Optional;

using Sharpmine.Domain;

namespace Sharpmine.Server.Infrastructure.Configuration;

public class DiskRegistryLoader : IRegistryLoader
{

    public ImmutableArray<Registry> Load()
    {
        string minecraftDir = Path.Combine(AppContext.BaseDirectory, "data", "minecraft");

        if (!Directory.Exists(minecraftDir))
        {
            throw new DirectoryNotFoundException($"Minecraft data directory not found at: {minecraftDir}");
        }

        List<Registry> loadedRegistries = [];

        var registryGroups = Directory.EnumerateFiles(minecraftDir, "*.json", SearchOption.AllDirectories)
            .GroupBy(file =>
            {
                string? registryDir = Path.GetDirectoryName(file);
                return (string.IsNullOrEmpty(registryDir))
                    ? string.Empty
                    : Path.GetRelativePath(minecraftDir, registryDir).Replace(Path.DirectorySeparatorChar, '/');
            })
            .Where(group => !string.IsNullOrEmpty(group.Key))
            .ToArray();

        foreach (var registryGroup in registryGroups)
        {
            string registryName = registryGroup.Key;
            string registryDir = Path.Combine(minecraftDir, registryName.Replace('/', Path.DirectorySeparatorChar));
            List<RegistryEntry> entries = [];

            foreach (string entry in registryGroup)
            {
                string entryPath = Path.GetRelativePath(registryDir, entry);
                string entryName = Path.GetFileNameWithoutExtension(entryPath).Replace(Path.DirectorySeparatorChar, '/');

                try
                {
                    var jsonNode = JsonNode.Parse(File.ReadAllBytes(entry));
                    var nbtTag = JsonToNbtConverter.Convert(jsonNode, string.Empty);
                    entries.Add(new RegistryEntry("minecraft:" + entryName, Option.Some(nbtTag)));
                }
                catch (Exception ex)
                {
                    throw new InvalidDataException($"Failed to parse registry file: {entry}", ex);
                }
            }

            if (entries.Count > 0)
            {
                loadedRegistries.Add(new Registry("minecraft:" + registryName, [.. entries]));
            }
        }

        return [.. loadedRegistries];
    }

}
