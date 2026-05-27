using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sharpmine.Domain.Registries.SubTypes.Enums;

public class StringEnumConverter<T> : JsonConverter<T> where T : struct, IStringEnum
{

    private static readonly Func<string, T> Factory;

    static StringEnumConverter()
    {
        var ctor = typeof(T).GetConstructor([typeof(string)]) ?? throw new InvalidOperationException($"Type {typeof(T).Name} must have a constructor accepting nothing but one string.");
        var param = Expression.Parameter(typeof(string), "value");
        var newExp = Expression.New(ctor, param);
        var lambda = Expression.Lambda<Func<string, T>>(newExp, param);
        Factory = lambda.Compile(); // (string value) => new T(value)
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return (value is not null) ? Factory(value) : default;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }

}
