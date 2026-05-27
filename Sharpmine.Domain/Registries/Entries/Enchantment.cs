using System.Text.Json;

using Sharpmine.Domain.Registries.SubTypes.Enums;
using Sharpmine.Domain.Registries.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Entries;

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
