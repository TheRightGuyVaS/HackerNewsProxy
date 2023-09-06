using HackerNewsProxy.Api.Infrastructure.Converters;
using Newtonsoft.Json;

namespace HackerNewsProxy.Api.Models;

public class ItemModel
{
#pragma warning disable CS8618
    public string Title { get; set; }
    public Uri Uri { get; set; }
    public string PostedBy { get; set; }
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime Time { get; set; }
    public int Score { get; set; }
    public int CommentCount { get; set; }
}
#pragma warning restore CS8618