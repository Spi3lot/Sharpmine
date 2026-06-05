using System.Text.Json;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record Dialog(JsonElement Messages);
