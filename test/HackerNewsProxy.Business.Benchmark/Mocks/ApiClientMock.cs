using HackerNews.Api.SDK.Entities;
using HackerNews.Api.SDK.Interfaces;

namespace HackerNewsProxy.Business.Benchmark.Mocks;

public class ApiClientMock : IApiClient
{
    private readonly SortedDictionary<int, List<ItemResponse>> _values;
    private readonly IDictionary<long, ItemResponse> _items;

    public ApiClientMock(int countOfItems)
    {
        _values = new SortedDictionary<int, List<ItemResponse>>(new Int32DescendantComparer());
        _items = new Dictionary<long, ItemResponse>();
        var r = new Random();
        
        for (var i = 0; i < countOfItems; i++)
        {
            var score = r.Next(countOfItems);
        
            if (_values.ContainsKey(score))
            {
                continue;
            }
        
            _values.Add(score, new List<ItemResponse>());
        }

        var scores = _values.Keys.ToArray();
        for (var i = 0; i < countOfItems; i++)
        {
            var item = new ItemResponse
            {
                Id = i + 1,
                Score = scores[r.Next(scores.Length)]
            };
            
            _items.Add(item.Id, item);
            _values[item.Score].Add(item);
        }
    }

    public Task<long[]> GetBestItemIdsAsync() => Task.FromResult(_values.SelectMany(x => x.Value.Select(t => t.Id)).ToArray());
    public Task<ItemResponse> GetItemByIdAsync(long id) => Task.FromResult(_items[id]);

    private class Int32DescendantComparer : IComparer<int>
    {
        public int Compare(int x, int y) => y.CompareTo(x);
    }
}