namespace Sharpmine.Server.Protocol.Packets.Configuration.Serverbound;

public partial record FinishConfigurationPacket : IStateTransition
{

    public ProtocolState NextState => ProtocolState.Play;

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader) => true;

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

}
