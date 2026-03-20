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

    // TODO: Read MaxPlayerCount from properties
    private readonly SemaphoreSlim _playerLobby = new(2, 2);

    private readonly TcpListener _listener = TcpListener.Create(port);

    private Task? _listenTask;

    private CancellationTokenSource? _cts;

    public event Action<ClientHandler>? ClientConnectionEstablished;

    public event Action<ClientHandler>? ClientConnectionTerminating;

    public event Action<ClientHandler>? ClientConnectionTerminated;

    public ConcurrentDictionary<Guid, ClientHandler> ActiveClientHandlers { get; } = [];

    public Task StartAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        logger.Log(LogLevel.Information, "Started server. Listening on port {Port}", port);

        _listener.Start();
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _listenTask = AcceptLoopAsync(_cts.Token);
        return Task.CompletedTask;
    }

    private async Task AcceptLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            bool semaphoreAcquired = false;

            try
            {
                await _playerLobby.WaitAsync(cancellationToken);
                semaphoreAcquired = true;
                var client = await _listener.AcceptTcpClientAsync(cancellationToken);
                HandleTcpClientAsync(client, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                if (semaphoreAcquired)
                {
                    _playerLobby.Release();
                }

                LogErrorWhileAccepting(ex);
            }
        }
    }

    private void HandleTcpClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                var handler = clientHandlerFactory.Create(client);
                SetupHandler(handler);
                await handler.HandleAsync(cancellationToken);
            }
            finally
            {
                _playerLobby.Release();
            }
        }, cancellationToken);
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
        _listener.Stop();

        if (_cts is not null)
        {
            await _cts.CancelAsync();
        }

        if (_listenTask is not null)
        {
            await Task.WhenAny(_listenTask, Task.Delay(-1, cancellationToken));
        }

        string gracefulness = (cancellationToken.IsCancellationRequested) ? "ungraceful" : "graceful";
        _cts?.Dispose();
        logger.Log(LogLevel.Information, "Stopped server {Gracefulness}ly", gracefulness);
    }

    [LoggerMessage(LogLevel.Information, "Stopped listening")]
    partial void LogStoppedListening();

    [LoggerMessage(LogLevel.Error, "An error occurred while trying to accept a client")]
    partial void LogErrorWhileAccepting(Exception ex);

}
