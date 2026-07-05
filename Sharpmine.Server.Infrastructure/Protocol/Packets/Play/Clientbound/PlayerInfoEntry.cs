using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.DataTypes.Components;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

public readonly record struct PlayerInfoEntry(
    Guid Uuid,

    // AddPlayer
    string? Name = null,
    GameProfileProperty[]? Properties = null,

    // InitializeChat TODO

    // UpdateGameMode
    GameMode? GameMode = null,

    // UpdateListed
    bool? Listed = null,

    // UpdateLatency
    int? Ping = null,

    // UpdateDisplayName
    Component? DisplayName = null,

    // UpdateListPriority
    int? ListPriority = null,

    // UpdateHat
    bool? HatVisible = null
);
