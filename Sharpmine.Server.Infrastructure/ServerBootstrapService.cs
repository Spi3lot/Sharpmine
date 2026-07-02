using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Sharpmine.Domain.Registries.Static;
using Sharpmine.Domain.Tags;
using Sharpmine.Server.Infrastructure.Configuration;
using Sharpmine.Server.Infrastructure.Protocol;

namespace Sharpmine.Server.Infrastructure;

public class ServerBootstrapService(
    DatapackLoader datapackLoader,
    IServiceProvider serviceProvider,
    ILogger<ServerBootstrapService> logger) : IHostedService
{

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await datapackLoader.ReloadAllAsync();
        logger.LogInformation("Dynamic registries filled successfully");

        // TODO: Add more registries to speed up initial connection even further
        _ = Blocks.Air;
        _ = BlockTags.Air;
        serviceProvider.GetRequiredService<NetworkRegistryCache>();
        serviceProvider.GetRequiredService<NetworkTagCache>();
        logger.LogInformation("Registry caches preloaded successfully");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}
