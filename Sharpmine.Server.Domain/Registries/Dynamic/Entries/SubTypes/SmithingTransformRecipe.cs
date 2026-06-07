using System.Text.Json;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries.SubTypes;

public sealed record SmithingTransformRecipe(
    JsonElement? Template,
    JsonElement Base,
    JsonElement? Addition,
    JsonElement Result) : Recipe;
