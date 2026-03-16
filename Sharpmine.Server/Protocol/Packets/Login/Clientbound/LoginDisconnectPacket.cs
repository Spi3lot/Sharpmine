using System.Net.Sockets;
using System.Text.Json;

using Sharpmine.Server.Protocol.Models;

namespace Sharpmine.Server.Protocol.Packets.Login.Clientbound;

public partial record LoginDisconnectPacket(TextComponent Reason)
{

    public Task SerializeContentAsync(NetworkStream stream, BinaryWriter writer)
    {
        return JsonSerializer.SerializeAsync(stream, Reason);
    }

}