using System.Collections.Concurrent;
using System.Net.Sockets;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Sharpmine.Server.Protocol;

namespace Sharpmine.Server;

public partial class ServerService(
    int port,
    ClientHandlerFactory clientHandlerFactory,
    ILogger<ServerService> logger) : IHostedService
{

    private Task _listenTask;

    public ConcurrentDictionary<Guid, ClientHandler> ActiveClientHandlers { get; } = [];

    public event Action<ClientHandler>? ClientConnectionEstablished;

    public event Action<ClientHandler>? ClientConnectionTerminating;

    public event Action<ClientHandler>? ClientConnectionTerminated;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.Log(LogLevel.Information, "Started server. Listening on port {Port}", port);
        var listener = TcpListener.Create(port);
        listener.Start();

        _listenTask = Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    await AcceptAndHandleTcpClientAsync(listener, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    LogStoppedListening();
                    return;
                }
                catch (Exception ex)
                {
                    LogErrorWhileAccepting(ex);
                }
            }
        }, cancellationToken);

        return Task.CompletedTask;
    }

    private async Task AcceptAndHandleTcpClientAsync(TcpListener listener, CancellationToken cancellationToken)
    {
        var client = await listener.AcceptTcpClientAsync(cancellationToken);
        var handler = clientHandlerFactory.Create(client);
        SetupHandler(handler);
        _ = Task.Run(() => handler.HandleAsync(cancellationToken), cancellationToken);
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

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.Log(LogLevel.Information, "Stopping server...");

        foreach (var handler in ActiveClientHandlers.Values)
        {
            await handler.DisposeAsync();
        }

        logger.Log(LogLevel.Information, "Stopped server gracefully");
    }

    [LoggerMessage(LogLevel.Information, "Stopped listening")]
    partial void LogStoppedListening();

    [LoggerMessage(LogLevel.Error, "An error occurred while trying to accept a client")]
    partial void LogErrorWhileAccepting(Exception ex);

}
