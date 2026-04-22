namespace Sharpmine.Server.Protocol.Packets.Login.Serverbound;

public partial record LoginAcknowledgedPacket : IStateTransition
{

    public ProtocolState NextState => ProtocolState.Configuration;

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader) => true;

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

}
