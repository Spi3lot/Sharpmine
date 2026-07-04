using Raspite.Tags;
using Raspite.Tags.Building;

using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.DataTypes.Components;
using Sharpmine.Domain.DataTypes.Nbt;

namespace Sharpmine.Server.Domain.Entities;

public class Player : ILoadableFromNbt<CompoundTag>, IConvertibleToNbt<CompoundTag>
{

    private readonly GameProfile _profile;

    private PlayerPosition _position;

    private PlayerRotation _rotation;

    private Player(in GameProfile profile)
    {
        _profile = profile;
    }

    public ref readonly GameProfile Profile => ref _profile;

    public ref PlayerPosition Position => ref _position;

    public ref PlayerRotation Rotation => ref _rotation;

    public GameMode GameMode { get; set; } = GameMode.Survival;

    public GameMode PreviousGameMode { get; set; } = GameMode.Undefined;

    public Component? DisplayName { get; set; }

    public int Ping { get; set; }

    public static async Task<Player> LoadAsync(GameProfile profile, string levelName)
    {
        var player = new Player(in profile);
        await player.LoadFromNbtFileAsync(player.GetNbtFilePath(levelName));
        return player;
    }

    public Task SaveAsync(string levelName)
    {
        return this.WriteToNbtFileAsync(
            path: GetNbtFilePath(levelName),
            rootName: "Player",
            initialBufferSize: 4096,
            backupExisting: true);
    }

    public void LoadFromNbt(CompoundTag tag)
    {
        GameMode = (GameMode) tag.GetInteger("playerGameType")!;
        PreviousGameMode = (GameMode) tag.GetInteger("previousPlayerGameType")!;
        Position = PlayerPosition.FromNbt(tag.GetList<DoubleTag>("Pos")!);
        Rotation = PlayerRotation.FromNbt(tag.GetList<FloatTag>("Rotation")!);
    }

    public CompoundTag ToNbt(string name = "")
    {
        return CompoundTagBuilder.Create(name)
            .Add(Position.ToNbt("Pos"))
            .Add(Rotation.ToNbt("Rotation"))
            .AddInteger((int) GameMode, "playerGameType")
            .AddInteger((int) PreviousGameMode, "previousPlayerGameType")
            .Add(DisplayName?.ToNbt("CustomName"))
            .Build();
    }

    private string GetNbtFilePath(string levelName) => Path.Combine(levelName, "playerdata", $"{Profile.Uuid}.dat");

}
