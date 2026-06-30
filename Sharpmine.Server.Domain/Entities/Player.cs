using Raspite.Tags;
using Raspite.Tags.Building;

using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.DataTypes.Components;

namespace Sharpmine.Server.Domain.Entities;

public class Player : ILoadableFromNbt<CompoundTag>, IConvertibleToNbt<CompoundTag>
{

    private Player(in GameProfile profile)
    {
        Profile = profile;
    }

    public GameProfile Profile { get; }

    public PlayerPosition Position { get; set; }

    public GameMode GameMode { get; set; } = GameMode.Survival;

    public GameMode PreviousGameMode { get; set; } = GameMode.Undefined;

    public Component? DisplayName { get; set; }

    public int Ping { get; set; }

    public static async Task<Player> LoadAsync(GameProfile profile, string levelName)
    {
        var player = new Player(in profile);
        string filePath = Path.Combine(levelName, "playerdata", $"{profile.Uuid}.dat");
        await player.LoadFromNbtFileAsync(filePath);
        return player;
    }

    public Task SaveAsync(string levelName)
    {
        return this.WriteToNbtFileAsync(
            path: Path.Combine(levelName, "playerdata", $"{Profile.Uuid}.dat"),
            rootName: "Player",
            initialBufferSize: 4096,
            backupExisting: true);
    }

    public void LoadFromNbt(CompoundTag tag)
    {
        GameMode = (GameMode) tag.GetInteger("playerGameType")!;
        PreviousGameMode = (GameMode) tag.GetInteger("previousPlayerGameType")!;
        Position = PlayerPosition.FromNbt(tag.GetList<DoubleTag>("Pos")!);
    }

    public CompoundTag ToNbt(string name = "")
    {
        return CompoundTagBuilder.Create(name)
            .Add(Position.ToNbt("Pos"))
            .AddInteger((int) GameMode, "playerGameType")
            .AddInteger((int) PreviousGameMode, "previousPlayerGameType")
            .Add(DisplayName?.ToNbt("CustomName"))
            .Build();
    }

}
