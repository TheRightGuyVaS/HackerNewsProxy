using AutoMapper;
using HackerNewsProxy.Business.Interfaces;
using HackerNewsProxy.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNewsProxy.Business;

public static class DependencyInjectionModule
{
    public static void ConfigureServices(this IServiceCollection serviceCollection, params Type[] automapperProfileTypes)
    {
        HackerNews.Api.SDK.DependencyInjectionModule.ConfigureServices(serviceCollection);
        serviceCollection.AddMemoryCache();
        serviceCollection.AddScoped<IItemService, ItemCacheService>();
        serviceCollection.AddSingleton(ConfigureMapper(automapperProfileTypes));
    }

    public static IMapper ConfigureMapper(params Type[] automapperProfileTypes)
    {
        var config = new MapperConfiguration(cfg =>
        {
            foreach (var type in automapperProfileTypes)
            {
                cfg.AddProfile(type);
            }
        });

        return config.CreateMapper();
    }
}