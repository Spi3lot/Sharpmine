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

    public ImmutableArray<RegistryDto> Get()
    {
        string baseDir = AppContext.BaseDirectory;
        string dataDir = Path.Combine(baseDir, "data");
        string registriesJsonPath = Path.Combine(baseDir, "generated", "reports", "registries.json");

        if (!Directory.Exists(dataDir))
        {
            throw new DirectoryNotFoundException($"Data directory not found at: {dataDir}");
        }

        HashSet<Identifier> knownRegistries = [];

        if (File.Exists(registriesJsonPath))
        {
            using var fileStream = File.OpenRead(registriesJsonPath);
            var jsonNode = JsonNode.Parse(fileStream);

            if (jsonNode is not null)
            {
                knownRegistries.UnionWith(jsonNode.AsObject().Select(node => new Identifier(node.Key)));
            }
        }
        else
        {
            logger.LogWarning("registries.json not found, falling back to protocol requirements only.");
        }

        foreach (var protocol in protocols)
        {
            knownRegistries.UnionWith(protocol.SynchronizedRegistryIds);
        }

        List<RegistryDto> loadedRegistries = [];

        foreach (var registryId in knownRegistries)
        {
            List<RegistryEntryDto> entries = [];

            foreach (string namespaceDir in Directory.EnumerateDirectories(dataDir))
            {
                string namespaceName = Path.GetFileName(namespaceDir);

                // e.g. data/minecraft/worldgen/biome
                string registryDir = Path.Combine(namespaceDir, registryId.Path.Replace('/', Path.DirectorySeparatorChar));

                if (!Directory.Exists(registryDir))
                {
                    logger.LogTrace("Registry folder not found for {RegistryId}, skipping it", registryId);
                    continue;
                }

                foreach (string entry in Directory.EnumerateFiles(registryDir, "*.json", SearchOption.AllDirectories))
                {
                    string entryPath = Path.GetRelativePath(registryDir, entry);
                    string entryName = entryPath.Replace(Path.DirectorySeparatorChar, '/').Replace(".json", string.Empty);
                    Identifier entryId = new Identifier(false, namespaceName, entryName);

                    try
                    {
                        using var fileStream = File.OpenRead(entry);
                        var jsonNode = JsonNode.Parse(fileStream);
                        var nbtTag = JsonToNbtConverter.Convert(jsonNode!);
                        entries.Add(new RegistryEntryDto(entryId, nbtTag!.SomeNotNull()));
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to parse registry file: {Entry}", entry);
                    }
                }
            }

            loadedRegistries.Add(new RegistryDto(registryId, [.. entries]));
        }

        return [.. loadedRegistries];
    }

}
