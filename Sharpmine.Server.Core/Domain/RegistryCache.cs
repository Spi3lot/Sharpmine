using System.Collections.Immutable;
using System.Text.Json.Nodes;

using Optional;

using Sharpmine.Server.Core.Protocol.DataTypes;
using Sharpmine.Server.Core.Protocol.Packets;
using Sharpmine.Server.Core.Protocol.Packets.Configuration.Clientbound;

namespace Sharpmine.Server.Core.Domain;

public class RegistryCache
{

    private static readonly string[] SynchronizedRegistryNames =
    [
        "banner_pattern",
        "chat_type",
        "damage_type",
        "dialog",
        "dimension_type",
        "enchantment",
        "instrument",
        "jukebox_song",
        "painting_variant",
        "trim_material",
        "trim_pattern",
        "worldgen/biome",
        "cat_variant",
        "chicken_variant",
        "cow_variant",
        "frog_variant",
        "pig_variant",
        "wolf_variant",
        "wolf_sound_variant"
    ];

    public RegistryCache()
    {
        List<PreSerializedPacket> packets = [];

        foreach (var registryFolder in GetRegistryFolders())
        {
            var entries =
                from file in registryFolder.Files
                let jsonNode = JsonNode.Parse(file.Bytes)
                let nbtTag = JsonToNbtConverter.Convert(jsonNode, string.Empty)
                select new RegistryEntry(file.Name, Option.Some(nbtTag));

            var packet = new RegistryDataPacket(registryFolder.RegistryId, entries.ToArray());
            packets.Add(PreSerializedPacket.Generate(packet));
        }

        Packets = [.. packets];
    }

    public ImmutableArray<PreSerializedPacket> Packets { get; }

    private static IEnumerable<RegistryFolderData> GetRegistryFolders()
    {
        string minecraftDir = Path.Combine(AppContext.BaseDirectory, "data", "minecraft");

        var synchronizedRegistries = (
            from registryName in SynchronizedRegistryNames
            let registryDir = Path.Combine(minecraftDir, registryName.Replace('/', Path.DirectorySeparatorChar))
            where Directory.Exists(registryDir)
            select (registryName, registryDir)).ToArray();

        foreach (var (registryName, registryDir) in synchronizedRegistries)
        {
            var files = (
                from file in Directory.EnumerateFiles(registryDir, "*.json", SearchOption.AllDirectories)
                let entryPath = Path.GetRelativePath(registryDir, file)
                let entryName = entryPath.Replace(Path.DirectorySeparatorChar, '/').Replace(".json", string.Empty)
                select new RegistryFileData("minecraft:" + entryName, File.ReadAllBytes(file))).ToArray();

            if (files.Length == 0)
            {
                throw new FileNotFoundException($"{registryDir} does not contain any entries.");
            }

            yield return new RegistryFolderData("minecraft:" + registryName, files);
        }
    }

    private record struct RegistryFolderData(string RegistryId, RegistryFileData[] Files);

    private record struct RegistryFileData(string Name, byte[] Bytes);

}
