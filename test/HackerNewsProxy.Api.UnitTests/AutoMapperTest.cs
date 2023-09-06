using System;
using AutoMapper;
using FluentAssertions;
using HackerNews.Api.SDK.Entities;
using HackerNewsProxy.Api.Infrastructure;
using HackerNewsProxy.Api.Models;
using Xunit;

namespace HackerNewsProxy.Api.UnitTests;

public class AutoMapperTest
{
    private readonly IMapper _mapper;
    
    public AutoMapperTest()
    {
        _mapper = Business.DependencyInjectionModule.ConfigureMapper(typeof(ApiMap));
    }

    [Fact]
    public void ShouldMapItemModel()
    {
        //arrange
        var itemResponse = new ItemResponse
        {
            Title = "title",
            Url = new Uri("http://example.com", UriKind.Absolute),
            By = "by",
            Time = DateTime.UtcNow,
            Score = 24,
            Descendants = 12
        };

        var expected = new ItemModel
        {
            Title = itemResponse.Title,
            Uri = itemResponse.Url,
            PostedBy = itemResponse.By,
            Time = itemResponse.Time,
            Score = itemResponse.Score,
            CommentCount = itemResponse.Descendants
        };

        //act
        var actual = _mapper.Map<ItemModel>(itemResponse);

        //assert
        actual.Should().BeEquivalentTo(expected);
    }
}