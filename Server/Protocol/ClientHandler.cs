using System.Net.Sockets;
using Sharpmine.Server.Protocol.Packets;

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

    public PacketSender PacketSender { get; } = new();

    public ProtocolState ProtocolState { get; set; }

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
                _ = await TryProcessNextPacketAsync(stream, reader, writer);
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

    public async Task<bool> TryProcessNextPacketAsync(NetworkStream stream, BinaryReader reader, BinaryWriter writer)
    {
        var packet = await IServerboundPacket.DeserializeAsync(this, reader);

        if (packet is null)
        {
            return false;
        }

        await Console.Out.WriteLineAsync($"Yay! Received packet ({packet.State}:0x{packet.Id:X2})");
        await packet.ProcessAsync(this, stream, reader, writer);
        return true;
    }

}