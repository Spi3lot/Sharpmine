using System.Text.Json;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries.SubTypes;

public sealed record ShapelessCraftingRecipe(
    bool ShowNotification,
    JsonElement Ingredients,
    JsonElement Result) : StandardRecipe;
