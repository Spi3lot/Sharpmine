using Sharpmine.Server.Protocol.Extensions;

namespace Sharpmine.Server.Protocol.Packets.Login.Clientbound;

public partial record LoginFinishedPacket(GameProfile Profile)
{

    public Task SerializeContentAsync(
        Stream stream,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        writer.Write(Profile.Uuid.ToByteArray());
        writer.Write(Profile.Username);

        writer.WritePrefixedArray(Profile.Properties, properties =>
        {
            writer.Write(properties.Name);
            writer.Write(properties.Value);
            writer.WritePrefixedOptional(properties.Signature, writer.Write);
        });

        return Task.CompletedTask;
    }

}

public record GameProfile(
    in Guid Uuid,
    string Username,
    GameProfileProperty[] Properties);

public record GameProfileProperty(
    string Name,
    string Value,
    string? Signature);
