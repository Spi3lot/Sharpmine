using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sharpmine.Domain;

public static class HostApplicationBuilderExtensions
{

    extension<TBuilder>(TBuilder builder) where TBuilder : IHostApplicationBuilder
    {

        public TBuilder AddDomainServices()
        {
            builder.Services.AddSingleton(sp =>
            {
                var loader = sp.GetRequiredService<IRegistryLoader>();
                return new RegistryCache(loader.Load());
            });

            return builder;
        }

    }

}
