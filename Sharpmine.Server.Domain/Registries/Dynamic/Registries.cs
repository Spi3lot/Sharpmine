using Sharpmine.Domain.Registries.Dynamic;
using Sharpmine.Server.Domain.Registries.Dynamic.Entries;

namespace Sharpmine.Server.Domain.Registries.Dynamic;

public class Registries : SynchronizedRegistries, IRegistries
{

    public DynamicRegistry<Advancement> Advancements { get; } = new();

    public DynamicRegistry<EnchantmentProvider> EnchantmentProviders { get; } = new();

    public DynamicRegistry<Recipe> Recipes { get; } = new();

}
