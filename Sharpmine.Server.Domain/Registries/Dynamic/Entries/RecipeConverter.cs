using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Sharpmine.Domain;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries;

public class RecipeConverter : JsonConverter<Recipe>
{

    public override Recipe? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (!root.TryGetProperty("type", out var typeElement) || typeElement.GetString() is not { } typeStr)
        {
            return null;
        }

        string[]? craftingPattern = null;
        Identifier? trimPattern = null;

        if (root.TryGetProperty("pattern", out var patternElement))
        {
            if (patternElement.ValueKind == JsonValueKind.Array)
            {
                craftingPattern = patternElement.Deserialize<string[]>(options);
            }
            else if (patternElement.ValueKind == JsonValueKind.String)
            {
                trimPattern = new Identifier(patternElement.GetString()!);
            }
        }

        int? cookingtime = root.TryGetProperty("cookingtime", out var ct)
            ? ct.GetInt32()
            : typeStr switch
            {
                "minecraft:smelting" => 200,
                "minecraft:campfire_cooking" or "minecraft:smoking" or "minecraft:blasting" => 100,
                _ => null
            };

        float experience = root.TryGetProperty("experience", out var exp) ? exp.GetSingle() : 0;
        string? group = root.TryGetProperty("group", out var grp) ? grp.GetString() : null;
        string category = root.TryGetProperty("category", out var cat) ? cat.GetString()! : "misc";
        bool showNotification = !root.TryGetProperty("show_notification", out var sn) || sn.GetBoolean();

        return new Recipe(
            Type: new Identifier(typeStr),
            CraftingPattern: craftingPattern,
            TrimPattern: trimPattern,
            Key: GetElement(root, "key"),
            Ingredients: GetElement(root, "ingredients"),
            Input: GetElement(root, "input"),
            Material: GetElement(root, "material"),
            Dye: GetElement(root, "dye"),
            Target: GetElement(root, "target"),
            Template: GetElement(root, "template"),
            Base: GetElement(root, "base"),
            Addition: GetElement(root, "addition"),
            Ingredient: GetElement(root, "ingredient"),
            Experience: experience,
            Cookingtime: cookingtime,
            Result: GetElement(root, "result"),
            Group: group,
            Category: category,
            ShowNotification: showNotification
        );
    }

    public override void Write(Utf8JsonWriter writer, Recipe value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize<object>(writer, value, options);
    }

    private static JsonElement? GetElement(JsonElement root, string propName)
    {
        return root.TryGetProperty(propName, out var prop) ? prop : null;
    }

}
