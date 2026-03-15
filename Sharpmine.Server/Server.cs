using System.Collections.Concurrent;
using System.Net.Sockets;

using Microsoft.Extensions.Logging;

using Sharpmine.Server.Protocol;

namespace Sharpmine.Server;

public class Server(int port, IClientHandlerFactory clientHandlerFactory, ILogger<Server> logger)
{

    public ConcurrentDictionary<Guid, ClientHandler> ActiveClientHandlers { get; } = [];

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
                ActiveClientHandlers[handler.Id] = handler;

                handler.ConnectionTerminated += () =>
                {
                    ActiveClientHandlers.Remove(handler.Id, out _);
                    ClientConnectionTerminated?.Invoke(handler);
                };

                ClientConnectionEstablished?.Invoke(handler);
                _ = Task.Run(handler.HandleAsync);
            }
        });
    }

}