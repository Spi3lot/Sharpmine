using System.Text.Json;
using System.Text.Json.Serialization;

using Sharpmine.Domain;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries;

[JsonConverter(typeof(RecipeConverter))]
public sealed record Recipe(
    Identifier Type,

    // Type is "minecraft:crafting_shaped"
    string[]? Pattern,
    JsonElement? Key,

    // Type is "minecraft:crafting_shapeless"
    JsonElement? Ingredients,

    // Type is "minecraft:crafting_transmute"
    JsonElement? Input,
    JsonElement? Material,

    // Type is "minecraft:crafting_dye"
    JsonElement? Dye,
    JsonElement? Target,

    // Type is "minecraft:smithing_transform" or "minecraft:smithing_trim"
    JsonElement? Template,
    JsonElement? Base,
    JsonElement? Addition,

    // Type is "minecraft:smelting", "minecraft:smoking", "minecraft:campfire_cooking",
    //         "minecraft:blasting" or minecraft:stonecutting"
    JsonElement? Ingredient,

    // Type is "minecraft:smelting", "minecraft:smoking", "minecraft:campfire_cooking" or
    //         "minecraft:blasting"
    float? Experience,
    int? Cookingtime,

    // Misc
    JsonElement? Result,
    string? Group,
    string? Category,
    bool? ShowNotification);
