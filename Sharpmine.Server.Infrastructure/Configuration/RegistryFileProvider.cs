using System.Collections.Immutable;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Optional;

using Sharpmine.Domain;
using Sharpmine.Domain.Registries;
using Sharpmine.Server.Infrastructure.Protocol.Versions;

namespace Sharpmine.Server.Infrastructure.Configuration;

public class RegistryFileProvider(
    IEnumerable<IProtocol> protocols,
    ILogger<RegistryFileProvider> logger) : IRegistryProvider
{

    public ImmutableArray<Registry> Get()
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

        HashSet<Identifier> knownRegistries =
        [
            .. JsonNode.Parse(File.ReadAllBytes(registriesJsonPath))!
                .AsObject()
                .Select(node => node.Key)
        ];

        foreach (var protocol in protocols)
        {
            knownRegistries.UnionWith(protocol.SynchronizedRegistryIds);
        }

        foreach (var registryId in knownRegistries)
        {
            string registryDir = Path.Combine(minecraftDir, registryId.Path.Replace('/', Path.DirectorySeparatorChar));

            if (!Directory.Exists(registryDir))
            {
                logger.LogTrace("Registry folder not found for {RegistryId}, skipping it", registryId);
                continue;
            }

            List<RegistryEntry> entries = [];

            foreach (string entry in Directory.EnumerateFiles(registryDir, "*.json", SearchOption.AllDirectories))
            {
                string entryPath = Path.GetRelativePath(registryDir, entry);
                Identifier entryName = entryPath.Replace(Path.DirectorySeparatorChar, '/').Replace(".json", string.Empty);

                try
                {
                    var jsonNode = JsonNode.Parse(File.ReadAllBytes(entry));
                    var nbtTag = JsonToNbtConverter.Convert(jsonNode);
                    entries.Add(new RegistryEntry(entryName, Option.Some(nbtTag)));
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
