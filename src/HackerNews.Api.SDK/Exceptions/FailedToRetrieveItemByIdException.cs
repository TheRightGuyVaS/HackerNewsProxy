using System.Net;

namespace HackerNews.Api.SDK.Exceptions;

public class FailedToRetrieveItemByIdException : Exception
{
    public FailedToRetrieveItemByIdException(long id, HttpStatusCode code, string content)
        : base($"Failed to retrieve item from HackerNews API by id '{id}' with {nameof(HttpStatusCode)} '{code}'")
    {
        Id = id;
        Code = code;
        Content = content;
    }
    
    public long Id { get; }
    public HttpStatusCode Code { get; }
    public string Content { get; }
}