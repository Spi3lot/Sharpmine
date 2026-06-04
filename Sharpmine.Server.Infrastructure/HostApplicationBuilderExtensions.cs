using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using Sharpmine.Domain;
using Sharpmine.Domain.Registries;
using Sharpmine.Domain.Registries.Dynamic;
using Sharpmine.Domain.Tags;
using Sharpmine.Server.Domain.Registries.Dynamic;
using Sharpmine.Server.Infrastructure.Configuration;
using Sharpmine.Server.Infrastructure.Protocol;
using Sharpmine.Server.Infrastructure.Protocol.Handlers;
using Sharpmine.Server.Infrastructure.Protocol.Packets;
using Sharpmine.Server.Infrastructure.Protocol.Versions;
using Sharpmine.Server.Infrastructure.Security;

namespace Sharpmine.Server.Infrastructure;

public static class HostApplicationBuilderExtensions
{

    extension<TBuilder>(TBuilder builder) where TBuilder : IHostApplicationBuilder
    {

        public TBuilder AddCoreServices()
        {
            builder.Configuration.AddIniFile(
                ServerConstants.FileNames.Properties,
                optional: true,
                reloadOnChange: true);

            builder.AddRegistryServices();
            builder.AddTagServices();
            builder.Services.AddDomainServices();
            builder.Services.AddSerilog();
            builder.Services.AddSingleton(builder.Configuration.Get<ServerProperties>() ?? new ServerProperties());
            builder.Services.AddSingleton<IProtocol, Protocol773>();
            builder.Services.AddSingleton<ProtocolRegistryManifest>();
            builder.Services.AddSingleton<PlayerAccessManager>();
            builder.Services.AddSingleton<PacketReceiver>();
            builder.Services.AddSingleton<PacketDispatcher>();
            builder.Services.AddTransient<PacketSerializer>();
            builder.Services.AddSingleton<ServerCapacityManager>();
            builder.Services.AddSingleton<DatapackLoader>();
            builder.Services.AddSingleton<ServerBootstrapper>();
            builder.Services.AddSingleton<ServerService>();
            builder.Services.AddHostedService<ServerService>(sp => sp.GetRequiredService<ServerService>());

            builder.Services.AddSingleton<Registries>();
            builder.Services.AddSingleton<IRegistries>(sp => sp.GetRequiredService<Registries>());
            builder.Services.AddSingleton<IServerRegistries>(sp => sp.GetRequiredService<Registries>());
            builder.Services.AddSingleton<ISynchronizedRegistries>(sp => sp.GetRequiredService<Registries>());

            builder.Services.Scan(scan => scan
                .FromAssemblyOf<ServerService>()
                .AddClasses(classes => classes.AssignableTo(typeof(IPacketHandler<>)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            return builder.AddFactoryServices();
        }

        public TBuilder AddRegistryServices()
        {
            builder.Services.AddSingleton<IRegistryProvider, RegistryFileProvider>();
            builder.Services.AddSingleton<NetworkRegistryCache>();
            return builder;
        }

        public TBuilder AddTagServices()
        {
            builder.Services.AddSingleton<ITagProvider, TagFileProvider>();
            builder.Services.AddSingleton<NetworkTagCache>();
            return builder;
        }

        public TBuilder AddFactoryServices()
        {
            builder.Services.AddSingleton<ClientHandlerFactory>();
            builder.Services.AddSingleton<TransmissionWorkerFactory>();
            builder.Services.AddSingleton<DispatchWorkerFactory>();
            return builder;
        }

    }

}
