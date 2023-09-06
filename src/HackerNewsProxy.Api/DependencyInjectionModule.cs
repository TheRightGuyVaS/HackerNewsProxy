using HackerNewsProxy.Api.Infrastructure;

namespace HackerNewsProxy.Api;

public static class DependencyInjectionModule
{
    public static void ConfigureServices(this IServiceCollection serviceCollection)
    {
        Business.DependencyInjectionModule.ConfigureServices(serviceCollection, typeof(ApiMap));
    }
}