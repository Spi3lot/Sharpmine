using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Login.Clientbound;

public partial record LoginFinishedPacket(in GameProfile Profile)
{

    public Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        writer.Write(Profile.Uuid.ToByteArray());
        writer.Write(Profile.Username);
        writer.Write(Profile.Properties.Name);
        writer.Write(Profile.Properties.Value);
        writer.WritePrefixedOptional(Profile.Properties.Signature);
        return Task.CompletedTask;
    }

}

public readonly record struct GameProfile(Guid Uuid, string Username, in GameProfileProperties Properties);

public readonly record struct GameProfileProperties(string Name, in string Value, string? Signature);
