using System.Text.Json;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries.SubTypes;

public sealed record TransmuteCraftingRecipe(
    bool ShowNotification,
    JsonElement Input,
    JsonElement Material,
    JsonElement Result) : StandardRecipe;
