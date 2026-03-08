using System.Net.Sockets;

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
                _ = TryDeserializeAndProcessPacket(reader, writer);
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

    private bool TryDeserializeAndProcessPacket(BinaryReader reader, BinaryWriter writer)
    {
        if (!IServerboundPacket.TryDeserialize(reader, out var packet, out int packetId, out int length))
        {
            Console.WriteLine($"Received unknown packet with id 0x{packetId:X2} and length {length}");
            return false;
        }

        Console.WriteLine($"Received packet 0x{packetId:X2} with length {length}");
        packet.Process(this, reader, writer);
        return true;
    }

}