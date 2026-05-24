using System.Collections.Immutable;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Optional;

using Sharpmine.Domain;
using Sharpmine.Domain.Registries;
using Sharpmine.Server.Infrastructure.Protocol.Versions;

namespace Sharpmine.Server.Infrastructure.Configuration;

public class DiskRegistryLoader(
    IEnumerable<IProtocol> protocols,
    ILogger<DiskRegistryLoader> logger) : IRegistryLoader
{

    public ImmutableArray<Registry> Load()
    {
        string baseDir = AppContext.BaseDirectory;
        string minecraftDir = Path.Combine(baseDir, "data", "minecraft");
        string registriesJsonPath = Path.Combine(baseDir, "generated", "reports", "registries.json");

        if (!Directory.Exists(minecraftDir))
        {
            throw new DirectoryNotFoundException($"Minecraft data directory not found at: {minecraftDir}");
        }


        if (!File.Exists(registriesJsonPath))
        {
            logger.LogWarning("registries.json not found, cannot load registries");
            return [];
        }

        List<Registry> loadedRegistries = [];

        var knownRegistries = JsonNode.Parse(File.ReadAllBytes(registriesJsonPath))!
            .AsObject()
            .Select(node => node.Key)
            .ToHashSet();

        foreach (var protocol in protocols)
        {
            knownRegistries.UnionWith(protocol.SynchronizedRegistryIds);
        }

        foreach (string registryId in knownRegistries)
        {
            string cleanId = registryId.Replace("minecraft:", string.Empty);
            string registryDir = Path.Combine(minecraftDir, cleanId.Replace('/', Path.DirectorySeparatorChar));

            if (!Directory.Exists(registryDir))
            {
                logger.LogTrace("Registry folder not found for {RegistryId}, skipping it", registryId);
                continue;
            }

            List<RegistryEntry> entries = [];

            foreach (string entry in Directory.EnumerateFiles(registryDir, "*.json", SearchOption.AllDirectories))
            {
                string entryPath = Path.GetRelativePath(registryDir, entry);
                string entryName = entryPath.Replace(Path.DirectorySeparatorChar, '/').Replace(".json", string.Empty);

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

            loadedRegistries.Add(new Registry(registryId, [.. entries]));
        }

        return [.. loadedRegistries];
    }

}
