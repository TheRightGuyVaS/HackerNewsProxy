using System.Runtime.CompilerServices;
using HackerNews.Api.SDK.Entities;
using HackerNews.Api.SDK.Interfaces;
using HackerNewsProxy.Business.Interfaces;
using Microsoft.Extensions.Caching.Memory;

[assembly:InternalsVisibleTo("HackerNewsProxy.Business.UnitTests")]
namespace HackerNewsProxy.Business.Services;

internal class ItemCacheService : IItemService
{
    private static readonly TimeSpan CacheExpirationPeriod = TimeSpan.FromMinutes(30);
    
    private readonly IApiClient _apiClient;
    private readonly IMemoryCache _cache;

    private long[]? _itemIds;

    public ItemCacheService(IApiClient apiClient, IMemoryCache cache)
    {
        _apiClient = apiClient;
        _cache = cache;
    }
    
    public async Task<ICollection<ItemResponse>> GetTopItemsByScoreAsync(int n)
    {
        _itemIds ??= await _apiClient.GetBestItemIdsAsync();

        var result = new ItemResponse[n];

        Parallel.For(0, Math.Min(n, _itemIds.Length), (i, _) =>
        {
            result[i] = GetItemResponseByIdAsync(_itemIds[i]).GetAwaiter().GetResult();
        });

        return result;
    }

    private async Task<ItemResponse> GetItemResponseByIdAsync(long id) =>
        //as we know that inner method cannot produce 'null' we can assume that 
        //we cannot receive 'null' from cache
        (await _cache.GetOrCreateAsync(id, ce =>
        {
            ce.AbsoluteExpirationRelativeToNow = CacheExpirationPeriod;
            return _apiClient.GetItemByIdAsync(id);
        }))!;
}