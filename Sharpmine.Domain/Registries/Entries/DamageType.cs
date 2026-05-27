using Sharpmine.Domain.Registries.SubTypes.Enums;

namespace Sharpmine.Domain.Registries.Entries;

public record DamageType(
    string MessageId,
    float Exhaustion,
    DamageScaling Scaling,
    DamageEffect? Effects,
    DeathMessageType? DeathMessageType);
