using System.Collections.Concurrent;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Sharpmine.Server.Protocol;
using Sharpmine.Server.Protocol.Packets.Status;

namespace Sharpmine.Server;

public partial class ServerService(
    int port,
    ClientHandlerFactory clientHandlerFactory,
    ILogger<ServerService> logger) : BackgroundService
{

    public event Action<ClientHandler>? ClientConnectionEstablished;

    public event Action<ClientHandler>? ClientConnectionTerminated;

    public ConcurrentDictionary<Guid, ClientHandler> ActiveClientHandlers { get; } = [];

    // TODO: Read from server properties
    public ServerStatus Status { get; } = new(
        Version: new StatusVersion("1.21.10", 773),
        Description: new StatusDescription("Heya"),
        Favicon: "data:image/png;base64,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");

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

                // TODO: Consider "hostile" ban check before even calling HandleAsync
                //       A "hostile" ban is even "stronger" than an IP ban.
                // if (IsIpBanned(client))
                // {
                //     client.Dispose();
                //     lobby.Release();
                //     continue;
                // }

                StartClientHandler(client, lobby, stoppingToken);
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

        var disconnectTasks = ActiveClientHandlers.Values
            .Select(handler => handler.DisconnectAsync("Server is shutting down."));

        await Task.WhenAll(disconnectTasks);
    }

    private void StartClientHandler(TcpClient client, SemaphoreSlim lobby, CancellationToken stoppingToken)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    client.Dispose();
                    return;
                }

                var handler = clientHandlerFactory.Create(client, this);
                SetupHandler(handler);
                await handler.HandleAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                LogErrorWhileHandling(ex, client.Client.RemoteEndPoint);
            }
            finally
            {
                lobby.Release();
            }
        }, CancellationToken.None); // To ensure lobby.Release() is always called
    }

    private void SetupHandler(ClientHandler handler)
    {
        ActiveClientHandlers[handler.Id] = handler;

        handler.Terminated += () =>
        {
            ActiveClientHandlers.Remove(handler.Id, out _);
            ClientConnectionTerminated?.Invoke(handler);
        };

        ClientConnectionEstablished?.Invoke(handler);
    }

}
