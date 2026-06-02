using System.Text.Json;

using Microsoft.Extensions.Logging;

using Sharpmine.Domain;
using Sharpmine.Domain.Registries.Dynamic;
using Sharpmine.Domain.Registries.Dynamic.Entries;

namespace Sharpmine.Server.Infrastructure.Configuration;

public class DatapackLoader(RegistryManager registries, ILogger<DatapackLoader> logger)
{

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
    };

    public async Task ReloadAllAsync()
    {
        registries.BannerPatterns.Load(await LoadRegistryAsync<BannerPattern>("banner_pattern"));
        registries.ChatTypes.Load(await LoadRegistryAsync<ChatType>("chat_type"));
        registries.DamageTypes.Load(await LoadRegistryAsync<DamageType>("damage_type"));
        registries.Dialogs.Load(await LoadRegistryAsync<Dialog>("dialog"));
        registries.DimensionTypes.Load(await LoadRegistryAsync<DimensionType>("dimension_type"));
        registries.Enchantments.Load(await LoadRegistryAsync<Enchantment>("enchantment"));
        registries.Instruments.Load(await LoadRegistryAsync<Instrument>("instrument"));
        registries.JukeboxSongs.Load(await LoadRegistryAsync<JukeboxSong>("jukebox_song"));
        registries.PaintingVariants.Load(await LoadRegistryAsync<PaintingVariant>("painting_variant"));
        registries.TrimMaterials.Load(await LoadRegistryAsync<TrimMaterial>("trim_material"));
        registries.TrimPatterns.Load(await LoadRegistryAsync<TrimPattern>("trim_pattern"));
        registries.Biomes.Load(await LoadRegistryAsync<Biome>("worldgen/biome"));
        registries.CatVariants.Load(await LoadRegistryAsync<CatVariant>("cat_variant"));
        registries.CowVariants.Load(await LoadRegistryAsync<CowVariant>("cow_variant"));
        registries.FrogVariants.Load(await LoadRegistryAsync<FrogVariant>("frog_variant"));
        registries.PigVariants.Load(await LoadRegistryAsync<PigVariant>("pig_variant"));
        registries.ChickenVariants.Load(await LoadRegistryAsync<ChickenVariant>("chicken_variant"));
        registries.WolfVariants.Load(await LoadRegistryAsync<WolfVariant>("wolf_variant"));
        registries.WolfSoundVariants.Load(await LoadRegistryAsync<WolfSoundVariant>("wolf_sound_variant"));
    }

    private async Task<Dictionary<Identifier, TDomain>> LoadRegistryAsync<TDomain>(string registryFolder)
    {
        var results = new Dictionary<Identifier, TDomain>();
        string baseDir = AppContext.BaseDirectory;
        string searchPath = Path.Combine(baseDir, "data", "minecraft", registryFolder);

        if (!Directory.Exists(searchPath))
        {
            logger.LogWarning("Directory {SearchPath} not found.", searchPath);
            return results;
        }

        foreach (string file in Directory.EnumerateFiles(searchPath, "*.json", SearchOption.AllDirectories))
        {
            string relativePath = Path.GetRelativePath(searchPath, file).Replace('\\', '/');
            string tagPath = relativePath.Replace(".json", string.Empty);

            await using var stream = File.OpenRead(file);
            var entry = await JsonSerializer.DeserializeAsync<TDomain>(stream, _jsonOptions);

            if (entry is not null)
            {
                var id = new Identifier("minecraft", tagPath);
                results[id] = entry;
            }
        }

        return results;
    }

}
