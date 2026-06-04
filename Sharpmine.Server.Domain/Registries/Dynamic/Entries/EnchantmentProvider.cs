using System.Text.Json;

using Sharpmine.Domain;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries;

public sealed record EnchantmentProvider(
    Identifier Type,

    // Type is "minecraft:single"
    Identifier? Enchantment,
    JsonElement? Level,

    // Type is "minecraft:enchantments_by_cost" or "minecraft:enchantments_by_cost_with_difficulty"
    JsonElement? Enchantments,
    JsonElement? Cost,
    int? MinCost,
    int? MaxCostSpan);
