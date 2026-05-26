using System.Collections.Frozen;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Sharpmine.Domain;
using Sharpmine.Domain.Registries;

namespace Sharpmine.Server.Infrastructure.Protocol;

public class ProtocolRegistryManifest
{

    public ProtocolRegistryManifest(RegistryCache registryCache, ILogger<ProtocolRegistryManifest> logger)
    {
        var protocolIds = new Dictionary<Identifier, Dictionary<Identifier, int>>();

        foreach (var registry in registryCache.Registries.Values)
        {
            protocolIds[registry.RegistryId] = [];

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
            StaticRegistries = FrozenSet<Identifier>.Empty;
            return;
        }

        HashSet<Identifier> staticRegistries = [];
        JsonObject? rootNode;

        using (var fileStream = File.OpenRead(registriesJsonPath))
        {
            rootNode = JsonNode.Parse(fileStream)!.AsObject();
        }

        foreach ((Identifier registryId, var registryNode) in rootNode)
        {
            bool @static = false;
            var entries = registryNode!["entries"]!.AsObject();

            foreach ((Identifier entryId, var entryNode) in entries)
            {
                var protocolIdNode = entryNode?["protocol_id"];

                if (protocolIdNode is null)
                {
                    continue;
                }

                if (!protocolIds.TryGetValue(registryId, out Dictionary<Identifier, int>? value))
                {
                    value = [];
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

    public FrozenDictionary<Identifier, FrozenDictionary<Identifier, int>> ProtocolIds { get; }

    public FrozenSet<Identifier> StaticRegistries { get; }

    public bool TryGetId(Identifier registryId, Identifier entryId, out int id)
    {
        id = 0;

        return ProtocolIds.TryGetValue(registryId, out var entries)
               && entries.TryGetValue(entryId, out id);
    }

    public bool IsStaticRegistry(Identifier registryId) => StaticRegistries.Contains(registryId);

}
