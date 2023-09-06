using HackerNews.Api.SDK.Entities;

namespace HackerNews.Api.SDK.Interfaces;

public interface IApiClient
{
    Task<long[]> GetBestItemIdsAsync();
    Task<ItemResponse> GetItemByIdAsync(long id);
}