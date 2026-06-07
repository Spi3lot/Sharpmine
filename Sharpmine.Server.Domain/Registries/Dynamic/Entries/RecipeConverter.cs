using System.Text.Json;
using System.Text.Json.Serialization;

using Sharpmine.Domain;
using Sharpmine.Server.Domain.Registries.Dynamic.Entries.SubTypes;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries;

public class RecipeConverter : JsonConverter<Recipe>
{

    public override Recipe Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var root = JsonElement.ParseValue(ref reader);

        if (!root.TryGetProperty("type", out var typeElement) || typeElement.GetString() is not { } typeStr)
        {
            throw new JsonException("Unspecified recipe type.");
        }

        Identifier typeId = typeStr;
        string category = root.TryGetProperty("category", out var cat) ? cat.GetString()! : (typeStr is "minecraft:smoking" or "minecraft:campfire_cooking" ? "food" : "misc");
        string? group = root.TryGetProperty("group", out var grp) ? grp.GetString() : null;
        bool showNotification = !root.TryGetProperty("show_notification", out var sn) || sn.GetBoolean();

        return typeStr switch
        {
            "minecraft:blasting" or "minecraft:smelting" or "minecraft:smoking" or "minecraft:campfire_cooking" => new CookingRecipe(
                root.GetProperty("ingredient"),
                root.TryGetProperty("cookingtime", out var ct) ? ct.GetInt32() : (typeStr == "minecraft:smelting" ? 200 : 100),
                root.GetProperty("result"),
                root.TryGetProperty("experience", out var exp) ? exp.GetSingle() : 0f) { Type = typeId, Category = category, Group = group },
            "minecraft:crafting_dye" => new DyeCraftingRecipe(
                showNotification,
                root.GetProperty("dye"),
                root.GetProperty("target"),
                root.GetProperty("result")) { Type = typeId, Category = category, Group = group },
            "minecraft:crafting_shaped" => new ShapedCraftingRecipe(
                showNotification,
                root.GetProperty("pattern").Deserialize<string[]>(options)!,
                root.GetProperty("key"),
                root.GetProperty("result")) { Type = typeId, Category = category, Group = group },
            "minecraft:crafting_shapeless" => new ShapelessCraftingRecipe(
                showNotification,
                root.GetProperty("ingredients"),
                root.GetProperty("result")) { Type = typeId, Category = category, Group = group },
            "minecraft:crafting_transmute" => new TransmuteCraftingRecipe(
                showNotification,
                root.GetProperty("input"),
                root.GetProperty("material"),
                root.GetProperty("result")) { Type = typeId, Category = category, Group = group },
            "minecraft:smithing_trim" => new SmithingTrimRecipe(
                GetOptionalProperty(root, "template"),
                root.GetProperty("base"),
                GetOptionalProperty(root, "addition"),
                new Identifier(root.GetProperty("pattern").GetString()!)) { Type = typeId },
            "minecraft:smithing_transform" => new SmithingTransformRecipe(
                GetOptionalProperty(root, "template"),
                root.GetProperty("base"),
                GetOptionalProperty(root, "addition"),
                root.GetProperty("result")) { Type = typeId },
            "minecraft:stonecutting" => new StonecuttingRecipe(
                root.GetProperty("ingredient"),
                root.GetProperty("result")) { Type = typeId },
            "minecraft:crafting_decorated_pot" => new DecoratedPotCraftingRecipe { Type = typeId, Category = category },
            _ when typeStr.StartsWith("minecraft:crafting_special_") => new SpecialCraftingRecipe { Type = typeId, Category = category },
            _ => throw new JsonException($"Unknown recipe type: {typeStr}.")
        };
    }

    public override void Write(Utf8JsonWriter writer, Recipe value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize<object>(writer, value, options);
    }

    private static JsonElement? GetOptionalProperty(JsonElement root, string propName)
    {
        return root.TryGetProperty(propName, out var prop) ? prop : null;
    }

}
