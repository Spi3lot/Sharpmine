using System.Text.Json;

using Sharpmine.Domain;
using Sharpmine.Domain.Providers.Int;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries;

public sealed record EnchantmentProvider(
    Identifier Type,

    // Type is "minecraft:single"
    Identifier? Enchantment,
    IIntProvider? Level,

    // Type is "minecraft:enchantments_by_cost" or "minecraft:enchantments_by_cost_with_difficulty"
    JsonElement? Enchantments,
    IIntProvider? Cost,
    int? MinCost,
    int? MaxCostSpan);
