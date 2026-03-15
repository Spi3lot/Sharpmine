using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

using Sharpmine.Server.Gui;
using Sharpmine.Server.Logging;
using Sharpmine.Server.Protocol;

namespace Sharpmine.Server;

public static class Program
{

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main(params string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        Console.WriteLine($"Current Environment: {builder.Environment.EnvironmentName}");

        bool nogui = builder.Configuration.GetValue<bool>("nogui");
        ushort port = builder.Configuration.GetValue<ushort?>("port") ?? 25565;
        var sink = new ListLogEventSink();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Sink(sink)
            .CreateLogger();

        builder.Services.AddSerilog();
        builder.Services.AddSingleton(sink);
        builder.Services.AddSingleton<Form1>();
        builder.Services.AddSingleton<IClientHandlerFactory, ClientHandlerFactory>();
        builder.Services.AddSingleton(sp => new Server(port, sp.GetRequiredService<IClientHandlerFactory>(), sp.GetRequiredService<ILogger<Server>>()));

        using var host = builder.Build();

        if (nogui || args.Contains("--nogui", StringComparer.OrdinalIgnoreCase)
                  || args.Contains("/nogui", StringComparer.OrdinalIgnoreCase))
        {
            var server = host.Services.GetRequiredService<Server>();
            server.HandleClientsInBackground();
            host.Run();
            return;
        }

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(host.Services.GetRequiredService<Form1>());
    }

}