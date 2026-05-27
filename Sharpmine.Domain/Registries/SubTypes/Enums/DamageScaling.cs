using System.Text.Json.Serialization;

namespace Sharpmine.Domain.Registries.SubTypes.Enums;

[JsonConverter(typeof(StringEnumConverter<DamageScaling>))]
public readonly record struct DamageScaling(string Value) : IStringEnum
{

    public static readonly DamageScaling Never = new("never");

    public static readonly DamageScaling WhenCausedByLivingNonPlayer = new("when_caused_by_living_non_player");

    public static readonly DamageScaling Always = new("always");

}
