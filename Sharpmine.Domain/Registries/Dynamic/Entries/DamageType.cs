using Sharpmine.Domain.Registries.Dynamic.SubTypes.Enums;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public record DamageType(
    string MessageId,
    float Exhaustion,
    DamageScaling Scaling,
    DamageEffect? Effects,
    DeathMessageType? DeathMessageType);
