using System.Threading.Channels;

using Sharpmine.Server.Infrastructure.Protocol.Packets;

namespace Sharpmine.Server.Infrastructure.Protocol.Extensions;

public static class ChannelExtensions
{

    extension(Channel)
    {

        public static Channel<IClientboundPacket> CreateClientbound()
        {
            return Channel.CreateBounded<IClientboundPacket>(new BoundedChannelOptions(8192)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleWriter = false,
                SingleReader = true,
                AllowSynchronousContinuations = false
            });
        }

        public static Channel<IServerboundPacket> CreateServerbound()
        {
            return Channel.CreateBounded<IServerboundPacket>(new BoundedChannelOptions(1024)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleWriter = true,
                SingleReader = true,
                AllowSynchronousContinuations = false
            });
        }

    }

}
