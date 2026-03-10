using System.Net.Sockets;
using Sharpmine.Server.Protocol;

namespace Sharpmine.Server;

public class Server(int port)
{

    public void HandleClientsInBackground()
    {
        var listener = TcpListener.Create(port);
        listener.Start();

        Task.Run(async () =>
        {
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = new ClientHandler(client).HandleAsync();
            }
        });
    }

}