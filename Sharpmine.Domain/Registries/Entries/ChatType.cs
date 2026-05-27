using Sharpmine.Domain.Registries.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Entries;

public record ChatType(
    ChatFormatting Chat,
    ChatFormatting Narration);
