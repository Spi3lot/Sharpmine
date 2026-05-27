using Microsoft.Extensions.DependencyInjection;

using Sharpmine.Domain.Registries;
using Sharpmine.Domain.Tags;

namespace Sharpmine.Domain;

public static class DomainServiceCollectionExtensions
{

    extension(IServiceCollection services)
    {

        public IServiceCollection AddDomainServices() => services
            .AddSingleton(sp =>
            {
                var provider = sp.GetRequiredService<IRegistryProvider>();
                return new RegistryCache(provider.Get());
            })
            .AddSingleton(sp =>
            {
                var provider = sp.GetRequiredService<ITagProvider>();
                return new TagCache(provider.Get());
            });

    }

}
