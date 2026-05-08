using System.Threading.Channels;

namespace Sharpmine.Server.Protocol.Packets;

public class ClientboundChannelWorker(
    Channel<IClientboundPacket> channel,
    ClientHandler client,
    NetworkStream stream,
    BinaryWriter writer,
    PacketTransceiver packetTransceiver) : ChannelWorker<IClientboundPacket>(channel)
{

    protected override Task ProcessAsync(IClientboundPacket currentItem, CancellationToken cancellationToken)
    {
        return packetTransceiver.TransmitAsync(currentItem, stream, writer, cancellationToken);
    }

    protected override Task OnErrorAsync(Exception ex, IClientboundPacket? currentItem)
    {
        client.LogErrorWhileTransmittingPacket(ex, currentItem);
        return client.AbortAsync();
    }

}
