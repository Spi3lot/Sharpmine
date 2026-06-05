using Sharpmine.Domain.DataTypes;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record Instrument(
    Identifier SoundEvent,
    float UseDuration,
    float Range,
    TextComponent Description);
