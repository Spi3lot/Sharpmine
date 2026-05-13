using System.Buffers;

using Sharpmine.Server.Core.Protocol.Attributes;
using Sharpmine.Server.Core.Protocol.Extensions;

namespace Sharpmine.Server.Core.Protocol.Packets.Abstract.Serverbound;

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

    public bool DeserializeContent(ref SequenceReader<byte> reader)
    {
        return reader.TryReadString(out _locale, 16)
               && reader.TryReadSByte(out _viewDistance)
               && reader.TryReadEnum(out _chatMode)
               && reader.TryReadBoolean(out _chatColors)
               && reader.TryReadEnum(out _displayedSkinParts)
               && reader.TryReadEnum(out _mainHand)
               && reader.TryReadBoolean(out _enableTextFiltering)
               && reader.TryReadBoolean(out _allowServerListings)
               && reader.TryReadEnum(out _particleStatus);
    }

}

public enum ChatMode
{

    Enabled = 0,

    CommandsOnly = 1,

    Hidden = 2,

}

[Flags]
public enum SkinParts
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
