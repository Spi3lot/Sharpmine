using Optional;

namespace Sharpmine.Domain.DataTypes;

public readonly record struct GameProfileProperty(
    string Name,
    string Value,
    Option<string> Signature);
