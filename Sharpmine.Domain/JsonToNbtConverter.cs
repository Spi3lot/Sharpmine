using System.Diagnostics.Contracts;
using System.Text.Json.Nodes;

using Raspite.Tags;

namespace Sharpmine.Domain;

public static class JsonToNbtConverter
{

    /// <summary>
    /// Converts a System.Text.Json Node into a Raspite NBT Tag.
    /// </summary>
    [Pure]
    public static Tag Convert(JsonNode? node, string name = "")
    {
        return node switch
        {
            JsonObject jsonObject => new CompoundTag(jsonObject.Select(kvp => Convert(kvp.Value, kvp.Key)).ToArray(), name),
            JsonArray jsonArray => new ListTag(jsonArray.Select(item => Convert(item, string.Empty)).ToArray(), name),
            JsonValue jsonValue when jsonValue.TryGetValue(out bool b) => new ByteTag((byte) (b ? 1 : 0), name),
            JsonValue jsonValue when jsonValue.TryGetValue(out int i) => new IntegerTag(i, name),
            JsonValue jsonValue when jsonValue.TryGetValue(out double d) => new DoubleTag(d, name),
            JsonValue jsonValue when jsonValue.TryGetValue(out string? s) => new StringTag(s, name),
            _ => new StringTag(string.Empty, name),
        };
    }

}
