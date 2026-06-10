using System.Buffers;
using System.ComponentModel;

using Raspite;
using Raspite.Tags;

using Sharpmine.Domain.DataTypes;

namespace Sharpmine.Server.Domain.Entities;

public class Player : INbtSerializable
{

    private Player(in GameProfile profile)
    {
        Profile = profile;
    }

    public GameProfile Profile { get; }

    public GameMode GameMode { get; set; } = GameMode.Survival;

    public GameMode PreviousGameMode { get; set; } = GameMode.Undefined;

    public Component? DisplayName { get; set; }

    public int Ping { get; set; }

    public static async Task<Player> LoadAsync(GameProfile profile, string levelName)
    {
        var player = new Player(in profile);
        string filePath = Path.Combine(levelName, "playerdata", $"{profile.Uuid}.dat");

        if (File.Exists(filePath) && TagSerializer.TryParse(await File.ReadAllBytesAsync(filePath), out var tag))
        {
            // TODO: player.GameMode = (GameMode) tag.GetInt("playerGameType");
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

        var arrayBufferWriter = new ArrayBufferWriter<byte>(4096);
        TagSerializer.Serialize(arrayBufferWriter, ToNbt());
        return File.WriteAllBytesAsync(filePath, arrayBufferWriter.WrittenMemory);
    }

    public Tag ToNbt(string name = "")
    {
        throw new NotImplementedException();
    }

}
