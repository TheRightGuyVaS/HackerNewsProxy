using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNewsProxy.Business.UnitTests.Helpers;

public static class MemoryCacheHelper
{
    public static IMemoryCache Cache = ServiceProvider().GetRequiredService<IMemoryCache>();

    private static IServiceProvider ServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddMemoryCache();
        return serviceCollection.BuildServiceProvider();
    }
}