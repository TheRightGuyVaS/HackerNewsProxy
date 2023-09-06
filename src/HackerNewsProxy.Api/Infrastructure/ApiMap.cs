using AutoMapper;
using HackerNews.Api.SDK.Entities;
using HackerNewsProxy.Api.Models;

namespace HackerNewsProxy.Api.Infrastructure;

public class ApiMap : Profile
{
    public ApiMap()
    {
        CreateMap<ItemResponse, ItemModel>(MemberList.Destination)
            .ForMember(dest => dest.PostedBy, opt => opt.MapFrom(src => src.By))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Descendants))
            .ForMember(dest => dest.Uri, opt => opt.MapFrom(src => src.Url));
    }
}