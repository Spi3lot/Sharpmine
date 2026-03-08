using System.Net.Sockets;

using Sharpmine.Server.Packets;
using Sharpmine.Server.Packets.Clientbound;
using Sharpmine.Server.Packets.Serverbound;

namespace Sharpmine.Server;

public class ClientHandler(TcpClient client)
{

    public ConnectionState ConnectionState { get; protected internal set; }

    protected internal PacketSerializer PacketSerializer { get; } = new();

    public async Task HandleAsync()
    {
        var reader = new BinaryReader(client.GetStream());
        var writer = new BinaryWriter(client.GetStream());
        
        try
        {
            while (client.Connected)
            {
                _ = await TryDeserializeAndProcessPacket(reader, writer);
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

    private async Task<bool> TryDeserializeAndProcessPacket(BinaryReader reader, BinaryWriter writer)
    {
        var deserialized = await IServerboundPacket.TryDeserialize(reader);
        
        if (deserialized.Packet is null)
        {
            Console.WriteLine("Received unknown packet");
            return false;
        }

        Console.WriteLine($"Received packet 0x{deserialized.PacketId:X2} with length {deserialized.Length}");
        await deserialized.Packet.Process(this, reader, writer);
        return true;
    }

}