using Sharpmine.Domain.Registries.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Entries;

public record JukeboxSong(
    Identifier SoundEvent,
    TrimDescription Description,
    float LengthInSeconds,
    int ComparatorOutput);
