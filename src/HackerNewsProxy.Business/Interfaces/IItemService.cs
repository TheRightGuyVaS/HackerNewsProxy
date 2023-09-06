using HackerNews.Api.SDK.Entities;

namespace HackerNewsProxy.Business.Interfaces;

public interface IItemService
{
    Task<ICollection<ItemResponse>> GetTopItemsByScoreAsync(int n);
}