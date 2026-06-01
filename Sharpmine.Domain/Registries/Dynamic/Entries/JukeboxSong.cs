using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public record JukeboxSong(
    Identifier SoundEvent,
    TextComponent Description,
    float LengthInSeconds,
    int ComparatorOutput);
