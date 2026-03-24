using System.Collections.Concurrent;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Sharpmine.Server.Protocol;

namespace Sharpmine.Server;

public partial class ServerService(
    int port,
    ClientHandlerFactory clientHandlerFactory,
    ILogger<ServerService> logger) : BackgroundService
{

    public event Action<ClientHandler>? ClientConnectionEstablished;

    public event Action<ClientHandler>? ClientConnectionTerminating;

    public event Action<ClientHandler>? ClientConnectionTerminated;

    public ConcurrentDictionary<Guid, ClientHandler> ActiveClientHandlers { get; } = [];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var lobby = new SemaphoreSlim(2, 2); // TODO: Use MaxPlayerCount from properties
        using var listener = TcpListener.Create(port);
        listener.Start();
        LogStartedServer(port);

        while (!stoppingToken.IsCancellationRequested)
        {
            bool semaphoreAcquired = false;

            try
            {
                await lobby.WaitAsync(stoppingToken);
                semaphoreAcquired = true;
                var client = await listener.AcceptTcpClientAsync(stoppingToken);
                HandleTcpClientAsync(client, lobby, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                if (semaphoreAcquired)
                {
                    lobby.Release();
                }

                LogErrorWhileAccepting(ex);
            }
        }

        LogStoppingServer();
    }

    private void HandleTcpClientAsync(TcpClient client, SemaphoreSlim lobby, CancellationToken stoppingToken)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                var handler = clientHandlerFactory.Create(client);
                SetupHandler(handler);
                await handler.HandleAsync(stoppingToken);
            }
            finally
            {
                lobby.Release();
            }
        }, stoppingToken);
    }

    private void SetupHandler(ClientHandler handler)
    {
        ActiveClientHandlers[handler.Id] = handler;
        handler.Disposing += () => ClientConnectionTerminating?.Invoke(handler);

        handler.Disposed += () =>
        {
            ActiveClientHandlers.Remove(handler.Id, out _);
            ClientConnectionTerminated?.Invoke(handler);
        };

        ClientConnectionEstablished?.Invoke(handler);
    }

    [LoggerMessage(LogLevel.Information, "Started server, listening on port {Port}")]
    partial void LogStartedServer(int port);

    [LoggerMessage(LogLevel.Information, "Stopping server...")]
    partial void LogStoppingServer();

    [LoggerMessage(LogLevel.Error, "An error occurred while trying to accept a client")]
    partial void LogErrorWhileAccepting(Exception ex);

}
