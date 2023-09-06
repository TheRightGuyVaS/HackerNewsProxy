namespace HackerNews.Api.SDK.Exceptions;

public class FailedToDeserializeJsonException : Exception
{
    public FailedToDeserializeJsonException(string json, Type targetType)
        : base($"Failed to deserialize json string into type '{targetType.FullName}'")
    {
        Json = json;
        TargetType = targetType;
    }
    
    public string Json { get; }
    public Type TargetType { get; }
}