using HackerNews.Api.SDK.Interfaces;
using HackerNews.Api.SDK.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Api.SDK;

public static class DependencyInjectionModule
{
    public static void ConfigureServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IApiClient, ApiClient>();
    }
}