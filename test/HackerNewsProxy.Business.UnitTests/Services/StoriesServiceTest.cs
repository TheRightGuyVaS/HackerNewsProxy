using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using HackerNews.Api.SDK.Entities;
using HackerNews.Api.SDK.Interfaces;
using HackerNewsProxy.Business.UnitTests.Helpers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using HackerNewsProxy.Business.Interfaces;
using HackerNewsProxy.Business.Services;
using Xunit;

namespace HackerNewsProxy.Business.UnitTests.Services;

public class StoriesServiceTest
{
    private readonly IApiClient _apiClient;
    private readonly IStoryService _storyService;

    public StoriesServiceTest()
    {
        _apiClient = Substitute.For<IApiClient>();
        _storyService =
            new StoryService(_apiClient, MemoryCacheHelper.Cache);
    }

    [Fact]
    public async Task ShouldCacheStories()
    {
        //arrange
        const long storyId = 1;
        _apiClient.GetBestItemIdsAsync().Returns(new[] { storyId });
        _apiClient.GetItemByIdAsync(storyId).Returns(new ItemResponse
        {
            Id = storyId,
            Score = int.MaxValue
        });

        //act
        await _storyService.GetTopStoriesAsync(1);
        var bestStories = await _storyService.GetTopStoriesAsync(1);

        //assert
        using (new AssertionScope())
        {
            bestStories.Should().NotBeEmpty();
            bestStories.Count.Should().Be(1);
            bestStories.Single().Id.Should().Be(storyId);

            await _apiClient.Received(1).GetBestItemIdsAsync();
            await _apiClient.Received(1).GetItemByIdAsync(storyId);
        }
    }

    [Fact]
    public void ShouldHandleNoStories()
    {
        //arrange
        _apiClient.GetBestItemIdsAsync().Returns(Array.Empty<long>());

        //act
        var act = async () => await _storyService.GetTopStoriesAsync(1);

        //assert
        act.Should().NotThrowAsync();
    }

    [Fact]
    public void ShouldHandleNullStories()
    {
        //arrange
        _apiClient.GetBestItemIdsAsync().ReturnsNull();

        //act
        var act = async () => await _storyService.GetTopStoriesAsync(1);

        //assert
        act.Should().NotThrowAsync();
    }

    [Fact]
    public void ShouldHandleStoryReturnedAsNull()
    {
        //arrange
        const long storyId = 1;
        _apiClient.GetBestItemIdsAsync().Returns(new[] { storyId });
        _apiClient.GetItemByIdAsync(storyId).ReturnsNull();

        //act
        var act = async () => await _storyService.GetTopStoriesAsync(1);

        //assert
        act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ShouldHandleStoriesWithSameScore()
    {
        //arrange
        const int score = 1;
        const long story1Id = 2;
        const long story2Id = 3;

        _apiClient.GetBestItemIdsAsync().Returns(new[] { story1Id, story2Id });
        _apiClient.GetItemByIdAsync(story1Id).Returns(new ItemResponse
        {
            Id = story1Id,
            Score = score
        });
        _apiClient.GetItemByIdAsync(story2Id).Returns(new ItemResponse
        {
            Id = story2Id,
            Score = score
        });
        
        //act
        ICollection<ItemResponse> stories = new List<ItemResponse>();
        var act = async () => stories = await _storyService.GetTopStoriesAsync(2);
        
        //assert
        using (new AssertionScope())
        {
            await act.Should().NotThrowAsync();
            stories.Count.Should().Be(2);
            stories.Select(x => x.Id).Should().BeEquivalentTo(new[] { story1Id, story2Id });
        }
    }
}