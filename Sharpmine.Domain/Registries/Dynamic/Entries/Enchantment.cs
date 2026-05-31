using System.Text.Json;

using Sharpmine.Domain.Registries.Dynamic.SubTypes.Enums;
using Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public record Enchantment(
    TrimDescription Description,
    Identifier SupportedItems,
    int Weight,
    int MaxLevel,
    EnchantmentCost MinCost,
    EnchantmentCost MaxCost,
    int AnvilCost,
    EquipmentSlot[] Slots,
    JsonElement Effects);
