using System.Text.Json;

namespace Sharpmine.Domain.Registries.Entries;

public record Dialog(JsonElement Messages);
