using Sharpmine.Domain.DataTypes.Components;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record JukeboxSong(
    Identifier SoundEvent,
    Component Description,
    float LengthInSeconds,
    int ComparatorOutput);
