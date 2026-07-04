using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Sharpmine.Domain.DataTypes;
using Sharpmine.Domain.Registries.Static;
using Sharpmine.Domain.Tags;
using Sharpmine.Server.Domain.Registries.Dynamic;
using Sharpmine.Server.Infrastructure.Configuration;
using Sharpmine.Server.Infrastructure.Protocol;

namespace Sharpmine.Server.Infrastructure;

public class ServerBootstrapService(
    DatapackLoader datapackLoader,
    IRegistries registries,
    IServiceProvider serviceProvider,
    ILogger<ServerBootstrapService> logger) : IHostedService
{

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await datapackLoader.ReloadAllAsync();
        logger.LogInformation("Dynamic registries filled successfully");

        _ = BlockTags.Air;
        new Chunk(registries.DimensionTypes["overworld"]).SetBlock(0, 0, 0, Blocks.Air.DefaultState.Id);
        serviceProvider.GetRequiredService<NetworkRegistryCache>();
        serviceProvider.GetRequiredService<NetworkTagCache>();
        logger.LogInformation("JIT and registry caches warmed up successfully");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}
