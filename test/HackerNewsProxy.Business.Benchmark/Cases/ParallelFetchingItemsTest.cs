using BenchmarkDotNet.Attributes;
using HackerNews.Api.SDK.Entities;
using HackerNews.Api.SDK.Interfaces;
using HackerNewsProxy.Business.Benchmark.Mocks;

namespace HackerNewsProxy.Business.Benchmark.Cases;

[SimpleJob]
public class ParallelFetchingItemsTest
{
    [Params(10, 100, 1000, 10000)]
    public int CountOfItems { get; set; }
    
    [GlobalSetup]
    public async Task GlobalSetup()
    {
        ApiClient = new ApiClientMock(CountOfItems);
        ItemIds = await ApiClient.GetBestItemIdsAsync();
    }

#pragma warning disable CS8618
    public IApiClient ApiClient { get; set; }
#pragma warning restore CS8618
    
#pragma warning disable CS8618
    private long[] ItemIds { get; set; }
#pragma warning restore CS8618

    [Benchmark]
    public async Task<ICollection<ItemResponse>> ManualTasks()
    {
        var result = new ItemResponse[CountOfItems];

        var tasks = new Task[CountOfItems];
        for (var i = 0; i < Math.Min(ItemIds.Length, CountOfItems); i++)
        {
            tasks[i] = SetItemToArrayById(i, result);
        }

        await Task.WhenAll(tasks);

        return result;
    }
    
    private async Task SetItemToArrayById(int i, ItemResponse[] arr)
    {
        arr[i] = await ApiClient.GetItemByIdAsync(ItemIds[i]);
    }

    [Benchmark]
    public Task<ICollection<ItemResponse>> ParallelFor()
    {
        var result = new ItemResponse[CountOfItems];

        Parallel.For(0, CountOfItems, (i, _) =>
        {
            result[i] = ApiClient.GetItemByIdAsync(ItemIds[i]).GetAwaiter().GetResult();
        });

        return Task.FromResult<ICollection<ItemResponse>>(result);
    }
}