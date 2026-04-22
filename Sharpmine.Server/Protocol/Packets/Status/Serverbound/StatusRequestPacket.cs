using Sharpmine.Server.Protocol.Packets.Status.Clientbound;

namespace Sharpmine.Server.Protocol.Packets.Status.Serverbound;

public partial record StatusRequestPacket
{

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader) => true;

    public ValueTask ProcessAsync(ClientHandler handler, CancellationToken cancellationToken)
    {
        var status = handler.Server.Status with
        {
            Players = new StatusPlayers(1337, 2)
            {
                Sample =
                [
                    new StatusPlayer("Deswegen", Guid.Parse("7e5abf92-0bf6-4b21-8c12-ad9a32720f3b")),
                    new StatusPlayer("Yes_Mc", Guid.Parse("ae2f02a6-001e-46b6-a659-5e016cf6e8e5"))
                ],
            },
        };

        var response = new StatusResponsePacket(status);
        handler.EnqueueClientboundPacket(response);
        return ValueTask.CompletedTask;
    }

}
