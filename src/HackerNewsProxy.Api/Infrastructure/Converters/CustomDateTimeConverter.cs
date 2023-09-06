using Newtonsoft.Json.Converters;

namespace HackerNewsProxy.Api.Infrastructure.Converters;

public class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter()
    {
        DateTimeFormat = "yyyy-MM-ddTHH:mm:sszzz";
    }
}