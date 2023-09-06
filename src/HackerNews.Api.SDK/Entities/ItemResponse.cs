using System.Text.Json.Serialization;
using HackerNews.Api.SDK.Entities.Enums;
using Newtonsoft.Json.Converters;

namespace HackerNews.Api.SDK.Entities;

#pragma warning disable CS8618
public class ItemResponse
{
    public long Id { get; set; }
    public string By { get; set; }
    public int Descendants { get; set; }
    public long[] Kids { get; set; }
    public int Score { get; set; }
    
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime Time { get; set; }
    
    public string Title { get; set; }
    public ItemType Type { get; set; }
    public Uri Url { get; set; }
}
#pragma warning restore CS8618