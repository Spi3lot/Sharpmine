using System.Text.Json;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries.SubTypes;

public sealed record DyeCraftingRecipe(
    bool ShowNotification,
    JsonElement Dye,
    JsonElement Target,
    JsonElement Result) : StandardRecipe;
