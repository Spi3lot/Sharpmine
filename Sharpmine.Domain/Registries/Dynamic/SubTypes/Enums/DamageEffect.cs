using System.Text.Json.Serialization;

namespace Sharpmine.Domain.Registries.Dynamic.SubTypes.Enums;

[JsonConverter(typeof(StringEnumConverter<DamageEffect>))]
public readonly record struct DamageEffect(string Value) : IStringEnum
{

    public static readonly DamageEffect Hurt = new("hurt");

    public static readonly DamageEffect Thorns = new("thorns");

    public static readonly DamageEffect Drowning = new("drowning");

    public static readonly DamageEffect Burning = new("burning");

    public static readonly DamageEffect Poking = new("poking");

    public static readonly DamageEffect Freezing = new("freezing");

}
