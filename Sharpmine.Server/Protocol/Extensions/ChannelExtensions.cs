using System.Threading.Channels;

using Sharpmine.Server.Protocol.Packets;

namespace Sharpmine.Server.Protocol.Extensions;

public static class ChannelExtensions
{

    extension(Channel)
    {

        public static Channel<IClientboundPacket> CreateClientbound()
        {
            return Channel.CreateBounded<IClientboundPacket>(new BoundedChannelOptions(10000)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleWriter = true,
                SingleReader = true,
                AllowSynchronousContinuations = false
            });
        }

        public static Channel<IServerboundPacket> CreateServerbound()
        {
            return Channel.CreateBounded<IServerboundPacket>(new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleWriter = true,
                SingleReader = true,
                AllowSynchronousContinuations = false
            });
        }

    }

}
