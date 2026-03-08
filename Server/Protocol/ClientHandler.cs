using System.Net.Sockets;

using Sharpmine.Server.Protocol.Packets.Clientbound;
using Sharpmine.Server.Protocol.Packets.Serverbound;

namespace Sharpmine.Server.Protocol;

public class ClientHandler(TcpClient client)
{

    /// Not thread-safe.
    /// Has to be updated whenever the line
    /// <code>_ = new ClientHandler(client).HandleAsync()</code>
    /// changes to something like
    /// <code>_ = Task.Run(() => new ClientHandler(client).HandleAsync());</code>
    public static IList<ClientHandler> ActiveHandlers { get; } = [];

    public TcpClient Client { get; } = client;

    public ConnectionState ConnectionState { get; protected internal set; }

    protected internal PacketSender PacketSender { get; } = new();

    public async Task HandleAsync()
    {
        ActiveHandlers.Add(this);
        var stream = Client.GetStream();
        var reader = new BinaryReader(stream);
        var writer = new BinaryWriter(stream);

        try
        {
            while (Client.Connected)
            {
                _ = await TryDeserializeAndProcessPacketAsync(stream, reader, writer);
            }
        }
        catch (IOException ioe) when (ioe.InnerException is SocketException)
        {
            await Console.Error.WriteLineAsync($"{Client.Client.LocalEndPoint} disconnected");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            throw;
        }
        finally
        {
            await writer.DisposeAsync();
            reader.Dispose();
            Client.Dispose();
            ActiveHandlers.Remove(this);
        }
    }

    private async Task<bool> TryDeserializeAndProcessPacketAsync(
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer
    )
    {
        var deserialized = await IServerboundPacket.DeserializeAsync(this, reader);
        await Console.Out.WriteLineAsync($"Current state is {ConnectionState}");

        if (deserialized.Packet is null)
        {
            await Console.Error.WriteLineAsync($"Huh? Received packet 0x{deserialized.PacketId:X2} with length {deserialized.Length}");
            return false;
        }

        await Console.Out.WriteLineAsync($"Yay! Received packet 0x{deserialized.PacketId:X2} with length {deserialized.Length}");
        await deserialized.Packet.ProcessAsync(this, stream, reader, writer);
        return true;
    }

}