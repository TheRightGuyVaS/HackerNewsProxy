using HackerNews.Api.SDK.Entities;

namespace HackerNewsProxy.Business.Interfaces;

public interface IStoryService
{
    Task<ICollection<ItemResponse>> GetTopStoriesAsync(int n);
}