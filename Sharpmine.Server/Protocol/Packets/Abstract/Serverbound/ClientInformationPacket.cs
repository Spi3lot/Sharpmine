using Sharpmine.Server.Protocol.Attributes;

namespace Sharpmine.Server.Protocol.Packets.Abstract.Serverbound;

public abstract partial record ClientInformationPacket
{

    [PacketProperty]
    private string _locale;

    [PacketProperty]
    private sbyte _viewDistance;

    [PacketProperty]
    private ChatMode _chatMode;

    [PacketProperty]
    private bool _chatColors;

    [PacketProperty]
    private SkinParts _displayedSkinParts;

    [PacketProperty]
    private Hand _mainHand;

    [PacketProperty]
    private bool _enableTextFiltering;

    [PacketProperty]
    private bool _allowServerListings;

    [PacketProperty]
    private ParticleStatus _particleStatus;

    public bool DeserializeContent(NetworkStream stream, BinaryReader reader)
    {
        _locale = reader.ReadString();
        _viewDistance = reader.ReadSByte();
        _chatMode = (ChatMode) reader.Read7BitEncodedInt();
        _chatColors = reader.ReadBoolean();
        _displayedSkinParts = (SkinParts) reader.ReadByte();
        _mainHand = (Hand) reader.Read7BitEncodedInt();
        _enableTextFiltering = reader.ReadBoolean();
        _allowServerListings = reader.ReadBoolean();
        _particleStatus = (ParticleStatus) reader.Read7BitEncodedInt();
        return true;
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
