using System.Net.Sockets;

namespace Sharpmine.Server.Packets.Serverbound;

[Packet(0x00, ConnectionState.Handshake)]
public class HandshakePacket : IServerboundPacket
{

    public int ProtocolVersion { get; set; }

    public string ServerAddress { get; set; } = null!;

    public ushort ServerPort { get; set; }

    public Intent Intent { get; set; }

    public Task DeserializeAsync(BinaryReader reader)
    {
        ProtocolVersion = reader.Read7BitEncodedInt();
        ServerAddress = reader.ReadString();
        ServerPort = reader.ReadUInt16();
        Intent = (Intent) reader.Read7BitEncodedInt();
        return Task.CompletedTask;
    }

    public Task ProcessAsync(
        ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer
    )
    {
        Console.WriteLine($"{ServerAddress}:{ServerPort} (protocol: {ProtocolVersion}, intent: {Intent})");
        handler.ConnectionState = (Intent == Intent.Status) ? ConnectionState.Status : ConnectionState.Login;
        return Task.CompletedTask;
    }

    public override string ToString()
    {
        return $"{ServerAddress}:{ServerPort} {Intent}, {ProtocolVersion}";
    }

}