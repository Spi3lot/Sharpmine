using System.Text.Json;

using Sharpmine.Domain.DataTypes.Components;
using Sharpmine.Domain.Registries.Dynamic.SubTypes.Enums;
using Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record Enchantment(
    Component Description,
    Identifier SupportedItems,
    int Weight,
    int MaxLevel,
    EnchantmentCost MinCost,
    EnchantmentCost MaxCost,
    int AnvilCost,
    EquipmentSlot[] Slots,
    JsonElement Effects);
