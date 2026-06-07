using System.Text.Json;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries.SubTypes;

public sealed record ShapedCraftingRecipe(
    bool ShowNotification,
    string[] Pattern,
    JsonElement Key,
    JsonElement Result) : StandardRecipe;
