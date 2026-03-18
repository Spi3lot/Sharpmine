using System.Net.Sockets;

using Serilog;

namespace Sharpmine.Server.Protocol.Packets.Handshake.Serverbound;

public partial record IntentionPacket
{

    public int ProtocolVersion { get; set; }

    public string ServerAddress { get; set; } = null!;

    public ushort ServerPort { get; set; }

    public Intent Intent { get; set; }

    public Task DeserializeContentAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
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
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        var oldState = handler.ProtocolState;
        var newState = (Intent == Intent.Status) ? ProtocolState.Status : ProtocolState.Login;
        handler.ProtocolState = newState;
        Log.Debug("Switched state from {OldState} to {NewState}", oldState, newState);
        return Task.CompletedTask;
    }

}

public enum Intent : byte
{

    Status = 1,

    Login = 2,

    Transfer = 3,

}