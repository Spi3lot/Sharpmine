namespace Sharpmine.Server.Protocol.Packets.Configuration.Serverbound;

public partial record FinishConfigurationPacket : IStateTransition
{

    public ProtocolState NextState => ProtocolState.Play;

    public ProtocolResult DeserializeContent(NetworkStream stream, BinaryReader reader) => ProtocolResult.Success;

    public ValueTask<ProtocolResult> ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(ProtocolResult.Success);
    }

}
