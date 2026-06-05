using Sharpmine.Domain.DataTypes;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public record Instrument(
    Identifier SoundEvent,
    float UseDuration,
    float Range,
    TextComponent Description);
