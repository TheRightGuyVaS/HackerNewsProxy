using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using HackerNews.Api.SDK.Entities;
using HackerNews.Api.SDK.Exceptions;
using HackerNews.Api.SDK.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Polly;
using Polly.Retry;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace HackerNews.Api.SDK.Services;

internal class ApiClient : IApiClient
{
    private static readonly Uri BaseAddress = new("https://hacker-news.firebaseio.com/v0/", UriKind.Absolute);

    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        Converters = new List<JsonConverter>
        {
            new StringEnumConverter(),
            new UnixDateTimeConverter()
        }
    };
    
    private static readonly AsyncRetryPolicy RetryOnTimeoutPolicy = Policy
        .Handle<HttpRequestException>(exception => exception.StatusCode == HttpStatusCode.GatewayTimeout)
        .WaitAndRetryAsync(10, _ => TimeSpan.FromMilliseconds(100));
    
    private readonly HttpClient _httpClient;
    
    public ApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = BaseAddress
        };

        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    public async Task<long[]> GetBestItemIdsAsync()
    {
        var response =
            (await RetryOnTimeoutPolicy.ExecuteAndCaptureAsync(async () =>
                await _httpClient.GetAsync("beststories.json"))).Result;
        var ids = await response.Content.ReadFromJsonAsync<long[]>();

        if (ids == null)
        {
            throw new FailedToDeserializeJsonException(await response.Content.ReadAsStringAsync(), typeof(long[]));
        }

        return ids;
    }

    public async Task<ItemResponse> GetItemByIdAsync(long id)
    {
        var response =
            (await RetryOnTimeoutPolicy.ExecuteAndCaptureAsync(
                async () => await _httpClient.GetAsync($"item/{id}.json"))).Result;

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new FailedToRetrieveItemByIdException(id, response.StatusCode,
                await response.Content.ReadAsStringAsync());
        }

        var stringContent = await response.Content.ReadAsStringAsync();
        var item = JsonConvert.DeserializeObject<ItemResponse>(stringContent, JsonSerializerSettings);

        if (item == null)
        {
            throw new FailedToDeserializeJsonException(await response.Content.ReadAsStringAsync(),
                typeof(ItemResponse));
        }

        return item;
    }
}