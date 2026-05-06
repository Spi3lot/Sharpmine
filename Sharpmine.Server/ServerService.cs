using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Sharpmine.Server.Configuration;
using Sharpmine.Server.Protocol;
using Sharpmine.Server.Security;

namespace Sharpmine.Server;

public partial class ServerService(
    ServerProperties properties,
    PlayerAccessManager playerAccessManager,
    ClientHandlerFactory clientHandlerFactory,
    ILogger<ServerService> logger) : BackgroundService
{

    public event Action<ClientHandler>? ClientConnectionEstablished;

    public event Action<ClientHandler>? ClientConnectionTerminated;

    public ConcurrentDictionary<Guid, ClientHandler> Clients { get; } = [];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var listener = TcpListener.Create(properties.ServerPort);
        listener.Start();
        LogStartedServer(properties.ServerPort);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var client = await listener.AcceptTcpClientAsync(stoppingToken);

                if (!TryDetermineClientIp(client.Client.RemoteEndPoint as IPEndPoint, out string? ip))
                {
                    LogClientIpIndeterminable();
                    client.Dispose();
                    continue;
                }

                if (JoinAccess.IpBlacklisted == playerAccessManager.EvaluateAccess(ip).Access)
                {
                    LogClientBlacklisted(ip);
                    client.Dispose();
                    continue;
                }

                StartClientHandler(client, ip, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                LogErrorWhileAccepting(ex);
            }
        }

        LogStoppingServer();

        var disconnectTasks = Clients.Values
            .Select(handler => handler.DisconnectAsync("Server is shutting down."));

        await Task.WhenAll(disconnectTasks);
    }

    private static bool TryDetermineClientIp(IPEndPoint? endpoint, [NotNullWhen(true)] out string? ip)
    {
        if (endpoint is null)
        {
            ip = null;
            return false;
        }

        var address = endpoint.Address;
        ip = (address.IsIPv4MappedToIPv6) ? address.MapToIPv4().ToString() : address.ToString();
        return true;
    }

    private void StartClientHandler(TcpClient client, string ip, CancellationToken stoppingToken)
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

                var handler = clientHandlerFactory.Create(ip, client, this);
                SetupHandler(handler);
                await handler.HandleAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                LogErrorWhileHandling(ex, ip);
            }
        }, stoppingToken);
    }

    private void SetupHandler(ClientHandler handler)
    {
        Clients[handler.Id] = handler;

        handler.Terminated += () =>
        {
            Clients.Remove(handler.Id, out _);
            ClientConnectionTerminated?.Invoke(handler);
        };

        ClientConnectionEstablished?.Invoke(handler);
    }

}
