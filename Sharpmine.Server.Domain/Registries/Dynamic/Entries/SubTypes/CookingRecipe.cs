using System.Text.Json;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries.SubTypes;

public sealed record CookingRecipe(
    JsonElement Ingredient,
    int CookingTime,
    JsonElement Result,
    float Experience) : StandardRecipe;
