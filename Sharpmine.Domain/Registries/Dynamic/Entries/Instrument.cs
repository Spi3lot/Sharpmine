using Sharpmine.Domain.DataTypes.Components;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record Instrument(
    Identifier SoundEvent,
    float UseDuration,
    float Range,
    Component Description);
