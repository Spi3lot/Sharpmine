using System.Buffers;

using Sharpmine.Domain.DataTypes;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;


namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Login.Clientbound;

public partial record LoginFinishedPacket(in GameProfile Profile)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteUuid(Profile.Uuid);
        writer.WriteString(Profile.Username);
        writer.WritePrefixedArray(Profile.Properties, static (writer, property) =>
        {
            writer.WriteString(property.Name);
            writer.WriteString(property.Value);
            writer.WritePrefixedOptional(property.Signature, static (writer, signature) => writer.WriteString(signature));
        });
    }

}
