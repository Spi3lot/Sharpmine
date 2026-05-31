using System.Text.Json.Serialization;

namespace Sharpmine.Domain.Registries.Dynamic.SubTypes.Enums;

[JsonConverter(typeof(StringEnumConverter<EquipmentSlot>))]
public readonly record struct EquipmentSlot(string Value) : IStringEnum
{

    public static readonly EquipmentSlot Mainhand = new("mainhand");

    public static readonly EquipmentSlot Offhand = new("offhand");

    public static readonly EquipmentSlot Head = new("head");

    public static readonly EquipmentSlot Chest = new("chest");

    public static readonly EquipmentSlot Legs = new("legs");

    public static readonly EquipmentSlot Feet = new("feet");

    public static readonly EquipmentSlot Body = new("body");

}
