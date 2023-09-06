using Microsoft.Extensions.DependencyInjection;

namespace HackerNewsProxy.Console.Infrastructure;

public static class DependencyInjectionModule
{
    public static void ConfigureServices(this IServiceCollection serviceCollection)
    {
        HackerNewsProxy.Business.DependencyInjectionModule.ConfigureServices(serviceCollection);
    }
}