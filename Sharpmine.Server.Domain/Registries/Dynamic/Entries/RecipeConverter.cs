using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries;

public class RecipeConverter : JsonConverter<Recipe>
{

    private sealed record RawRecipe(
        string? Type,
        string[]? Pattern,
        JsonElement? Key,
        JsonElement? Ingredients,
        JsonElement? Input,
        JsonElement? Material,
        JsonElement? Dye,
        JsonElement? Target,
        JsonElement? Template,
        JsonElement? Base,
        JsonElement? Addition,
        JsonElement? Ingredient,
        float? Experience,
        int? Cookingtime,
        JsonElement? Result,
        string? Group,
        string? Category,
        bool? ShowNotification
    );

    public override Recipe? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var raw = JsonSerializer.Deserialize<RawRecipe>(ref reader, options);
        if (raw is null || string.IsNullOrWhiteSpace(raw.Type)) return null;

        int? cookingTime = raw.Cookingtime ?? (raw.Type switch
        {
            "minecraft:smelting" => 200,
            "minecraft:campfire_cooking" or "minecraft:smoking" or "minecraft:blasting" => 100,
            _ => null
        });

        float experience = raw.Experience ?? 0;
        string category = raw.Category ?? "misc";
        bool showNotification = raw.ShowNotification ?? true;

        return new Recipe(
            Type: raw.Type,
            Pattern: raw.Pattern,
            Key: raw.Key,
            Ingredients: raw.Ingredients,
            Input: raw.Input,
            Material: raw.Material,
            Dye: raw.Dye,
            Target: raw.Target,
            Template: raw.Template,
            Base: raw.Base,
            Addition: raw.Addition,
            Ingredient: raw.Ingredient,
            Experience: experience,
            Cookingtime: cookingTime,
            Result: raw.Result,
            Group: raw.Group,
            Category: category,
            ShowNotification: showNotification
        );
    }

    public override void Write(Utf8JsonWriter writer, Recipe value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }

}
