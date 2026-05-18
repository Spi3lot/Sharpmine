using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using Sharpmine.Server.Infrastructure;

namespace Sharpmine.Server.Wpf;

public static class Program
{

    [STAThread]
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            var builder = Host.CreateApplicationBuilder(args);
            var sink = new ListLogEventSink();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Sink(sink)
                .CreateLogger();

            builder.AddCoreServices();
            builder.Services.AddSingleton(sink);
            builder.Services.AddSingleton<MainWindow>();
            builder.Services.AddSingleton<MainViewModel>();

            using var host = builder.Build();
            host.Start();

            var app = new System.Windows.Application();
            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            app.Run(mainWindow);

            host.StopAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The application encountered a fatal error and crashed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

}
