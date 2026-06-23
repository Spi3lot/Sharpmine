using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Text.Json.Nodes;

using Raspite.Tags;

namespace Sharpmine.Domain;

public static class JsonToNbtConverter
{

    /// <summary>
    /// Converts a System.Text.Json Element into a Raspite NBT Tag.
    /// Returns null if the Element is null.
    /// </summary>
    [Pure]
    public static ITag? Convert(JsonElement element, string name = "")
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => CompoundTag.Create(element.EnumerateObject().Select(prop => Convert(prop.Value, prop.Name)).OfType<ITag>(), name),
            JsonValueKind.Array => ListTag.Create(element.EnumerateArray().Select(item => Convert(item, string.Empty)).OfType<ITag>(), name),
            JsonValueKind.True => new ByteTag(true, name),
            JsonValueKind.False => new ByteTag(false, name),
            JsonValueKind.Number => (element.TryGetInt32(out int i))
                ? new IntegerTag(i, name)
                : new DoubleTag(element.GetDouble(), name),
            JsonValueKind.String => new StringTag(element.GetString()!, name),
            _ => null
        };
    }

    /// <summary>
    /// Converts a System.Text.Json Node into a Raspite NBT Tag.
    /// Returns null if the Node is null.
    /// </summary>
    [Pure]
    public static ITag? Convert(JsonNode? node, string name = "")
    {
        return node switch
        {
            JsonObject jsonObject => CompoundTag.Create(jsonObject.Select(kvp => Convert(kvp.Value, kvp.Key)).OfType<ITag>(), name),
            JsonArray jsonArray => ListTag.Create(jsonArray.Select(item => Convert(item, string.Empty)).OfType<ITag>(), name),
            JsonValue jsonValue when jsonValue.TryGetValue(out bool b) => new ByteTag((byte) (b ? 1 : 0), name),
            JsonValue jsonValue when jsonValue.TryGetValue(out int i) => new IntegerTag(i, name),
            JsonValue jsonValue when jsonValue.TryGetValue(out double d) => new DoubleTag(d, name),
            JsonValue jsonValue when jsonValue.TryGetValue(out string? s) => new StringTag(s, name),
            _ => null
        };
    }

}
