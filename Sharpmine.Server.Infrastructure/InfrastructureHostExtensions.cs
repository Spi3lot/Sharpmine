using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Sharpmine.Server.Infrastructure.Protocol;

namespace Sharpmine.Server.Infrastructure;

public static class InfrastructureHostExtensions
{

    extension(IHost host)
    {

        public void PreloadCaches()
        {
            host.Services.GetRequiredService<NetworkRegistryCache>();
            host.Services.GetRequiredService<NetworkTagCache>();
        }

    }

}
