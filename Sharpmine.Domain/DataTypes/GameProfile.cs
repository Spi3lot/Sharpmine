namespace Sharpmine.Domain.DataTypes;

public readonly record struct GameProfile(
    Guid Uuid,
    string Username,
    GameProfileProperty[] Properties);
