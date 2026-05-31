using Microsoft.Extensions.DependencyInjection;

using Sharpmine.Domain.Registries;
using Sharpmine.Domain.Registries.Dynamic;
using Sharpmine.Domain.Tags;

namespace Sharpmine.Domain;

public static class DomainServiceCollectionExtensions
{

    extension(IServiceCollection services)
    {

        public IServiceCollection AddDomainServices() => services
            .AddSingleton<RegistryManager>()
            .AddSingleton(sp => new RegistryCache(sp.GetRequiredService<IRegistryProvider>().Get()))
            .AddSingleton(sp => new TagCache(sp.GetRequiredService<ITagProvider>().Get()));

    }

}
