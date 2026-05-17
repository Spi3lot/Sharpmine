using System.Buffers;
using System.IO.Pipelines;

using Microsoft.Extensions.Logging;

using Sharpmine.Server.Core.Protocol.Extensions;
using Sharpmine.Server.Core.Protocol.Packets;

namespace Sharpmine.Server.Core.Protocol;

public partial class PacketReceiver(ILogger<PacketReceiver> logger)
{

    public async Task<(bool KeepAlive, IServerboundPacket? Packet)> ReceiveAsync(
        ProtocolState state,
        PipeReader pipeReader,
        CancellationToken cancellationToken)
    {
        var result = await pipeReader.ReadAsync(cancellationToken);

        if (result.IsCanceled)
        {
            LogPendingPipeReadCanceled();
            return (false, null);
        }

        var buffer = result.Buffer;

        /*
         * I am aware this check is not perfect and leads to a false positive
         * when an IntentionPacket with a length of 254 bytes is sent.
         * However for that to occur, the client would need to enter a hostname
         * of exactly 246 characters in length, which is highly unlikely to ever
         * be attempted by anyone, at least not in a serious manner.
         *
         * I therefore declare this a negligible side effect.
         */
        if (state == ProtocolState.Handshake && buffer.Length > 0 && buffer.FirstSpan[0] == 0xFE)
        {
            LogReceivedLegacyPing();
            return (false, null);
        }

        var lengthPrefixReader = new SequenceReader<byte>(buffer);

        if (!lengthPrefixReader.TryReadVarInt(out int length, out int lengthPrefixLength))
        {
            if (lengthPrefixLength < 0)
            {
                LogReceivedCorruptedPacketLength(state);
                return (false, null);
            }

            pipeReader.AdvanceTo(buffer.Start, buffer.End);
            return (!result.IsCompleted, null);
        }

        if (buffer.Length < lengthPrefixLength + length)
        {
            pipeReader.AdvanceTo(buffer.Start, buffer.End);
            return (!result.IsCompleted, null);
        }

        if (length == 0)
        {
            LogReceivedEmptyPacket(state);
            return (false, null);
        }

        var packetSlice = buffer.Slice(lengthPrefixLength, length);
        var packetReader = new SequenceReader<byte>(packetSlice);

        if (!packetReader.TryReadVarInt(out int packetId, out _))
        {
            LogReceivedCorruptedPacketId(state, length);
            return (false, null);
        }

        if (!ServerboundPacketRegistry.TryCreatePacket(state, packetId, out var packet))
        {
            LogReceivedUnknownPacket(state, packetId, length);
            pipeReader.AdvanceTo(packetSlice.End);
            return (true, null);
        }

        try
        {
            if (!packet.DeserializeContent(ref packetReader))
            {
                LogDeserializeViolation(packet);
                return (false, null);
            }
        }
        catch (NotImplementedException)
        {
            LogDeserializeNotImplemented(packet);
            pipeReader.AdvanceTo(packetSlice.End);
            return (packet is not IStateTransition, null);
        }

        LogReceivedPacket(packet, length);
        pipeReader.AdvanceTo(packetSlice.End);
        return (true, packet);
    }

}
