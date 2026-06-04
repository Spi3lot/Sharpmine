using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Sharpmine.Server.Infrastructure.Configuration;

namespace Sharpmine.Server.Infrastructure;

public class ServerBootstrapService(
    DatapackLoader datapackLoader,
    ILogger<ServerBootstrapService> logger) : IHostedService
{

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Loading datapacks and filling registries...");
        await datapackLoader.ReloadAllAsync();
        logger.LogInformation("Registries loaded successfully.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}
