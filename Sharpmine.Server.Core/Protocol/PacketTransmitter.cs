using System.Buffers;
using System.IO.Pipelines;

using Microsoft.Extensions.Logging;

using Sharpmine.Server.Core.Protocol.Extensions;
using Sharpmine.Server.Core.Protocol.Packets;

namespace Sharpmine.Server.Core.Protocol;

public partial class PacketTransmitter(ILogger<PacketTransmitter> logger)
{

    private readonly ArrayBufferWriter<byte> _arrayBufferWriter = new();

    public async Task TransmitAsync(
        IClientboundPacket packet,
        PipeWriter pipeWriter,
        CancellationToken cancellationToken)
    {
        _arrayBufferWriter.Clear();
        _arrayBufferWriter.WriteVarInt(packet.Id);

        try
        {
            packet.SerializeContent(_arrayBufferWriter);
        }
        catch (NotImplementedException)
        {
            LogSerializeNotImplemented(packet);
            return;
        }

        int packetLength = _arrayBufferWriter.WrittenCount;
        LogTransmittingPacket(packet, packet.State, packet.Id, packetLength);

        pipeWriter.WriteVarInt(packetLength);
        pipeWriter.Write(_arrayBufferWriter.WrittenSpan);
        await pipeWriter.FlushAsync(cancellationToken);
    }

}
