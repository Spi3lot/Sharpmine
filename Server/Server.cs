using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

using Microsoft.Extensions.Logging;

using Sharpmine.Server.Protocol;

namespace Sharpmine.Server;

public partial class Server(int port, IClientHandlerFactory clientHandlerFactory, ILogger<Server> logger)
{

    public ConcurrentDictionary<EndPoint, ClientHandler> ActiveClientHandlers { get; } = [];

    public event Action<ClientHandler>? ClientConnectionEstablished;

    public event Action<ClientHandler>? ClientConnectionTerminated;

    public void HandleClientsInBackground()
    {
        var listener = TcpListener.Create(port);
        listener.Start();

        Task.Run(async () =>
        {
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                var handler = clientHandlerFactory.Create(client);

                if (!ActiveClientHandlers.TryAdd(client.Client.RemoteEndPoint!, handler))
                {
                    LogClientAlreadyConnected(client.Client.RemoteEndPoint);
                    continue;
                }

                handler.ConnectionTerminated += () =>
                {
                    ActiveClientHandlers!.Remove(client.Client.RemoteEndPoint, out var removedHandler);
                    ClientConnectionTerminated?.Invoke(removedHandler!);
                };

                ClientConnectionEstablished?.Invoke(handler);
                _ = Task.Run(handler.HandleAsync);
            }
        });
    }

    [LoggerMessage(LogLevel.Error, "Client {EndPoint} is already connected")]
    partial void LogClientAlreadyConnected(EndPoint? endPoint);

}