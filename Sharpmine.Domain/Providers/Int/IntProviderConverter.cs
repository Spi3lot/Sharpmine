using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sharpmine.Domain.Providers.Int;

public class IntProviderConverter : JsonConverter<IIntProvider>
{

    public override IIntProvider Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return new ConstantIntProvider(reader.GetInt32());
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var root = JsonElement.ParseValue(ref reader);

            if (!root.TryGetProperty("type", out var typeElement))
            {
                throw new JsonException("IntProvider missing 'type'.");
            }

            string type = typeElement.GetString()?.Replace("minecraft:", string.Empty) ?? string.Empty;

            return type switch
            {
                "constant" => new ConstantIntProvider(
                    root.GetProperty("value").GetInt32()),

                "uniform" => new UniformIntProvider(
                    root.GetProperty("min_inclusive").GetInt32(),
                    root.GetProperty("max_inclusive").GetInt32()),

                "biased_to_bottom" => new BiasedToBottomIntProvider(
                    root.GetProperty("min_inclusive").GetInt32(),
                    root.GetProperty("max_inclusive").GetInt32()),

                "clamped" => new ClampedIntProvider(
                    root.GetProperty("value").Deserialize<IIntProvider>(options)!,
                    root.GetProperty("min_inclusive").GetInt32(),
                    root.GetProperty("max_inclusive").GetInt32()),

                "clamped_normal" => new ClampedNormalIntProvider(
                    root.GetProperty("mean").GetSingle(),
                    root.GetProperty("deviation").GetSingle(),
                    root.GetProperty("min_inclusive").GetInt32(),
                    root.GetProperty("max_inclusive").GetInt32()),

                "weighted_list" => new WeightedListIntProvider(
                    ParseWeightedList(root.GetProperty("distribution"), options)),

                _ => throw new JsonException($"Unsupported IntProvider type: {type}")
            };
        }

        throw new JsonException($"Invalid IntProvider JSON token: {reader.TokenType}");
    }

    private static ImmutableArray<WeightedDistributionEntry> ParseWeightedList(JsonElement distributionArray, JsonSerializerOptions options)
    {
        var builder = ImmutableArray.CreateBuilder<WeightedDistributionEntry>();

        foreach (var element in distributionArray.EnumerateArray())
        {
            builder.Add(new WeightedDistributionEntry(
                element.GetProperty("weight").GetInt32(),
                element.GetProperty("data").Deserialize<IIntProvider>(options)!
            ));
        }

        return builder.ToImmutable();
    }

    public override void Write(Utf8JsonWriter writer, IIntProvider value, JsonSerializerOptions options)
    {
        if (value is ConstantIntProvider constant)
        {
            writer.WriteNumberValue(constant.Value);
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("type", value.Type);
        using var document = JsonSerializer.SerializeToDocument(value, value.GetType(), options);

        foreach (var property in document.RootElement.EnumerateObject())
        {
            property.WriteTo(writer);
        }

        writer.WriteEndObject();
    }

}
