using Microsoft.Extensions.Hosting;

using Serilog;

using Sharpmine.Server.Core;

namespace Sharpmine.Server.Console;

public static class Program
{

    public static void Main(params string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            var builder = Host.CreateApplicationBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.AddServiceDefaults();
            builder.AddCoreServices();

            using var host = builder.Build();
            host.Run();
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
