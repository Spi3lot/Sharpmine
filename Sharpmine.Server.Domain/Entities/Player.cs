using System.Buffers;
using System.ComponentModel;

using Raspite;
using Raspite.Tags;
using Raspite.Tags.Building;

using Sharpmine.Domain;
using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.DataTypes.Components;
using Sharpmine.Domain.Extensions;

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

        if (File.Exists(filePath) && TagSerializer.TryParse<CompoundTag>(await File.ReadAllBytesAsync(filePath), out var tag))
        {
            player.LoadFromNbt(tag);
        }

        return player;
    }

    public Task SaveAsync(string levelName)
    {
        string filePath = Path.Combine(levelName, "playerdata", $"{Profile.Uuid}.dat");

        if (File.Exists(filePath))
        {
            File.Move(filePath, $"{filePath}_old", overwrite: true);
        }

        using var pooledWriter = new PooledByteBufferWriter(4096);
        TagSerializer.Serialize(pooledWriter, ToNbt());
        return File.WriteAllBytesAsync(filePath, pooledWriter.WrittenMemory);
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
