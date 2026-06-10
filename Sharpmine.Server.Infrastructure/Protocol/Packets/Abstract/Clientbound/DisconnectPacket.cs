using System.Buffers;

using Sharpmine.Domain.DataTypes.Components;
using Sharpmine.Server.Infrastructure.Protocol.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Abstract.Clientbound;

public abstract partial record DisconnectPacket
{

    public Component Reason { get; init; } = null!;

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteNbt(Reason.ToNbt(), network: true);
    }

}
