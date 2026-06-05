using Sharpmine.Domain.Registries.Dynamic.SubTypes.Structures;

namespace Sharpmine.Domain.Registries.Dynamic.Entries;

public sealed record ChatType(
    ChatFormatting Chat,
    ChatFormatting Narration);
