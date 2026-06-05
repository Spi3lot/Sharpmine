using Sharpmine.Domain.DataTypes;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record JukeboxSong(
    Identifier SoundEvent,
    TextComponent Description,
    float LengthInSeconds,
    int ComparatorOutput);
