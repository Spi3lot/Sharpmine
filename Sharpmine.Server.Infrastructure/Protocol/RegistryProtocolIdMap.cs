using System.Collections.Frozen;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Sharpmine.Domain.Registries;

namespace Sharpmine.Server.Infrastructure.Protocol;

public class RegistryProtocolIdMap
{

    private readonly FrozenDictionary<string, FrozenDictionary<string, int>> _map;

    private readonly FrozenSet<string> _staticRegistries;

    public RegistryProtocolIdMap(RegistryCache registryCache, ILogger<RegistryProtocolIdMap> logger)
    {
        var map = new Dictionary<string, Dictionary<string, int>>();

        foreach (var registry in registryCache.Registries.Values)
        {
            map[registry.RegistryId] = new Dictionary<string, int>();

            for (int i = 0; i < registry.Entries.Length; i++)
            {
                map[registry.RegistryId][registry.Entries[i].EntryId] = i;
            }
        }

        string registriesJsonPath = Path.Combine(AppContext.BaseDirectory, "generated", "reports", "registries.json");

        if (!File.Exists(registriesJsonPath))
        {
            logger.LogWarning("registries.json not found! Static tags (like blocks/items) will fail to resolve.");
            _map = map.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value.ToFrozenDictionary());
            _staticRegistries = FrozenSet<string>.Empty;
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

                if (!map.TryGetValue(registryId, out Dictionary<string, int>? value))
                {
                    value = new Dictionary<string, int>();
                    map[registryId] = value;
                }

                value[entryId] = protocolIdNode.GetValue<int>();
                @static = true;
            }

            if (@static)
            {
                staticRegistries.Add(registryId);
            }
        }

        _map = map.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value.ToFrozenDictionary());
        _staticRegistries = staticRegistries.ToFrozenSet();
    }

    public bool TryGetId(string registryId, string entryId, out int id)
    {
        id = 0;

        return _map.TryGetValue(registryId, out var entries)
               && entries.TryGetValue(entryId, out id);
    }

    public bool IsStaticRegistry(string registryId) => _staticRegistries.Contains(registryId);

}
