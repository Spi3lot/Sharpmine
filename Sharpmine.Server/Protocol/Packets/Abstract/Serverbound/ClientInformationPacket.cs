namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract record ClientInformationPacket : IServerboundPacket
{

    public abstract ProtocolState State { get; }

    public abstract int Id { get; }

    public string Locale { get; set; } = null!;

    public sbyte ViewDistance { get; set; }

    public ChatMode ChatMode { get; set; }

    public bool ChatColors { get; set; }

    public SkinParts DisplayedSkinParts { get; set; }

    public Hand MainHand { get; set; }

    public bool EnableTextFiltering { get; set; }

    public bool AllowServerListings { get; set; }

    public ParticleStatus ParticleStatus { get; set; }

    public static Task DeserializeAsync(
        NetworkStream stream,
        BinaryReader reader,
        CancellationToken cancellationToken)
    {
        Locale = reader.ReadString();
        ViewDistance = reader.ReadSByte();
        ChatMode = (ChatMode) reader.Read7BitEncodedInt();
        ChatColors = reader.ReadBoolean();
        DisplayedSkinParts = (SkinParts) reader.ReadByte();
        MainHand = (Hand) reader.Read7BitEncodedInt();
        EnableTextFiltering = reader.ReadBoolean();
        AllowServerListings = reader.ReadBoolean();
        ParticleStatus = (ParticleStatus) reader.Read7BitEncodedInt();
        return Task.CompletedTask;
    }

    public Task ProcessAsync(
        ClientHandler handler,
        NetworkStream stream,
        BinaryReader reader,
        BinaryWriter writer,
        CancellationToken cancellationToken)
    {
        handler.Information = this;
        return Task.CompletedTask;
    }

}

public enum ChatMode
{

    Enabled = 0,

    CommandsOnly = 1,

    Hidden = 2,

}

[Flags]
public enum SkinParts : byte
{

    Cape = 0x01,

    Jacket = 0x02,

    LeftSleeve = 0x04,

    RightSleeve = 0x08,

    LeftPantsLeg = 0x10,

    RightPantsLeg = 0x20,

    Hat = 0x40,

}

public enum Hand
{

    Left = 0,

    Right = 1,

}

public enum ParticleStatus
{

    All = 0,

    Decreased = 1,

    Minimal = 2,

}
