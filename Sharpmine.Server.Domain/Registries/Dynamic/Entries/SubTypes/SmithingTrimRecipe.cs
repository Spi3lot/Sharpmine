using System.Text.Json;

using Sharpmine.Domain;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries.SubTypes;

public sealed record SmithingTrimRecipe(
    JsonElement? Template,
    JsonElement Base,
    JsonElement? Addition,
    Identifier Pattern) : Recipe;
