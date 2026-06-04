using Sharpmine.Domain.Registries.Dynamic;
using Sharpmine.Server.Domain.Registries.Dynamic.Entries;

namespace Sharpmine.Server.Domain.Registries.Dynamic;

public interface IServerRegistries
{

    DynamicRegistry<Advancement> Advancements { get; }

    DynamicRegistry<EnchantmentProvider> EnchantmentProviders { get; }

    DynamicRegistry<Recipe> Recipes { get; }

    // TODO: loot_table, trial_spawner, worldgen (everything except worldgen/biome)

}
