using System.Text.Json.Serialization;

namespace Sharpmine.Domain.Registries.SubTypes.Enums;

[JsonConverter(typeof(StringEnumConverter<DeathMessageType>))]
public readonly record struct DeathMessageType(string Value) : IStringEnum
{

    public static readonly DeathMessageType Default = new("default");

    public static readonly DeathMessageType FallVariants = new("fall_variants");

    public static readonly DeathMessageType IntentionalGameDesign = new("intentional_game_design");

}
