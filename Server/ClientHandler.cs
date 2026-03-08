using System.Net.Sockets;
using Sharpmine.Server.Packets;
using Sharpmine.Server.Packets.Clientbound;
using Sharpmine.Server.Packets.Serverbound;

namespace Sharpmine.Server;

public class ClientHandler(TcpClient client)
{

    public ConnectionState ConnectionState { get; protected internal set; }

    protected internal PacketSender PacketSender { get; } = new();

    public async Task HandleAsync()
    {
        var stream = client.GetStream();
        var reader = new BinaryReader(stream);
        var writer = new BinaryWriter(stream);

        try
        {
            while (client.Connected)
            {
                _ = await TryDeserializeAndProcessPacketAsync(stream, reader, writer);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await writer.DisposeAsync();
            reader.Dispose();
            client.Dispose();
        }
    }

    private async Task<bool> TryDeserializeAndProcessPacketAsync(
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer
    )
    {
        var deserialized = await IServerboundPacket.DeserializeAsync(this, reader);

        if (deserialized.Packet is null)
        {
            Console.WriteLine("Received unknown packet");
            return false;
        }

        Console.WriteLine($"Received packet 0x{deserialized.PacketId:X2} with length {deserialized.Length}");
        await deserialized.Packet.ProcessAsync(this, stream, reader, writer);
        return true;
    }

}