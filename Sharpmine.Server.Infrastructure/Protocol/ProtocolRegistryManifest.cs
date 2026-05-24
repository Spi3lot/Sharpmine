using System.Collections.Frozen;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Sharpmine.Domain.Registries;

namespace Sharpmine.Server.Infrastructure.Protocol;

public class ProtocolRegistryManifest
{

    public ProtocolRegistryManifest(RegistryCache registryCache, ILogger<ProtocolRegistryManifest> logger)
    {
        var protocolIds = new Dictionary<string, Dictionary<string, int>>();

        foreach (var registry in registryCache.Registries.Values)
        {
            protocolIds[registry.RegistryId] = new Dictionary<string, int>();

            for (int i = 0; i < registry.Entries.Length; i++)
            {
                protocolIds[registry.RegistryId][registry.Entries[i].EntryId] = i;
            }
        }

        string registriesJsonPath = Path.Combine(AppContext.BaseDirectory, "generated", "reports", "registries.json");

        if (!File.Exists(registriesJsonPath))
        {
            logger.LogWarning("registries.json not found! Static tags (like blocks/items) will fail to resolve.");
            ProtocolIds = protocolIds.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value.ToFrozenDictionary());
            StaticRegistries = FrozenSet<string>.Empty;
            return;
        }

        HashSet<string> staticRegistries = [];
        var rootNode = JsonNode.Parse(File.ReadAllBytes(registriesJsonPath))!.AsObject();

        foreach (var (registryId, registryNode) in rootNode)
        {
            bool @static = false;
            var entries = registryNode!["entries"]!.AsObject();

            foreach (var (entryId, entryNode) in entries)
            {
                var protocolIdNode = entryNode?["protocol_id"];

                if (protocolIdNode is null)
                {
                    continue;
                }

                if (!protocolIds.TryGetValue(registryId, out Dictionary<string, int>? value))
                {
                    value = new Dictionary<string, int>();
                    protocolIds[registryId] = value;
                }

                value[entryId] = protocolIdNode.GetValue<int>();
                @static = true;
            }

            if (@static)
            {
                staticRegistries.Add(registryId);
            }
        }

        ProtocolIds = protocolIds.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value.ToFrozenDictionary());
        StaticRegistries = staticRegistries.ToFrozenSet();
    }

    public FrozenDictionary<string, FrozenDictionary<string, int>> ProtocolIds { get; }

    public FrozenSet<string> StaticRegistries { get; }

    public bool TryGetId(string registryId, string entryId, out int id)
    {
        id = 0;

        return ProtocolIds.TryGetValue(registryId, out var entries)
               && entries.TryGetValue(entryId, out id);
    }

    public bool IsStaticRegistry(string registryId) => StaticRegistries.Contains(registryId);

}
