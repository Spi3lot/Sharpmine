namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

[Flags]
public enum PlayerActions : byte
{

    AddPlayer = 0x01,

    InitializeChat = 0x02,

    UpdateGameMode = 0x04,

    UpdateListed = 0x08,

    UpdateLatency = 0x10,

    UpdateDisplayName = 0x20,

    UpdateListPriority = 0x40,

    UpdateHat = 0x80

}
