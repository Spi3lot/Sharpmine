using System.Text.Json;

using Sharpmine.Domain;

namespace Sharpmine.Server.Domain.Registries.Dynamic.Entries;

public sealed record Advancement(
    Identifier? Parent,
    JsonElement Criteria,
    JsonElement? Display,
    string[][]? Requirements,
    JsonElement? Rewards);
