using System.Text.Json;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries.SubTypes;

public sealed record StonecuttingRecipe(
    JsonElement Ingredient,
    JsonElement Result) : Recipe;
