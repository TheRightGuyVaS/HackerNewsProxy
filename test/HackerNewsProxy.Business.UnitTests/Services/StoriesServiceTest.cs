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
    private readonly IItemService _itemService;

    public StoriesServiceTest()
    {
        _apiClient = Substitute.For<IApiClient>();
        _itemService =
            new ItemCacheService(_apiClient, MemoryCacheHelper.Cache);
    }

    [Fact]
    public async Task ShouldCacheItems()
    {
        //arrange
        const long itemId = 1;
        _apiClient.GetBestItemIdsAsync().Returns(new[] { itemId });
        _apiClient.GetItemByIdAsync(itemId).Returns(new ItemResponse
        {
            Id = itemId,
            Score = int.MaxValue
        });

        //act
        await _itemService.GetTopItemsByScoreAsync(1);
        var bestStories = await _itemService.GetTopItemsByScoreAsync(1);

        //assert
        using (new AssertionScope())
        {
            bestStories.Should().NotBeEmpty();
            bestStories.Count.Should().Be(1);
            bestStories.Single().Id.Should().Be(itemId);

            await _apiClient.Received(1).GetBestItemIdsAsync();
            await _apiClient.Received(1).GetItemByIdAsync(itemId);
        }
    }

    [Fact]
    public void ShouldHandleNoItems()
    {
        //arrange
        _apiClient.GetBestItemIdsAsync().Returns(Array.Empty<long>());

        //act
        var act = async () => await _itemService.GetTopItemsByScoreAsync(1);

        //assert
        act.Should().NotThrowAsync();
    }

    [Fact]
    public void ShouldHandleNullItems()
    {
        //arrange
        _apiClient.GetBestItemIdsAsync().ReturnsNull();

        //act
        var act = async () => await _itemService.GetTopItemsByScoreAsync(1);

        //assert
        act.Should().NotThrowAsync();
    }

    [Fact]
    public void ShouldHandleItemReturnedAsNull()
    {
        //arrange
        const long itemId = 1;
        _apiClient.GetBestItemIdsAsync().Returns(new[] { itemId });
        _apiClient.GetItemByIdAsync(itemId).ReturnsNull();

        //act
        var act = async () => await _itemService.GetTopItemsByScoreAsync(1);

        //assert
        act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ShouldHandleItemsWithSameScore()
    {
        //arrange
        const int score = 1;
        const long item1Id = 2;
        const long item2Id = 3;

        _apiClient.GetBestItemIdsAsync().Returns(new[] { item1Id, item2Id });
        _apiClient.GetItemByIdAsync(item1Id).Returns(new ItemResponse
        {
            Id = item1Id,
            Score = score
        });
        _apiClient.GetItemByIdAsync(item2Id).Returns(new ItemResponse
        {
            Id = item2Id,
            Score = score
        });
        
        //act
        ICollection<ItemResponse> items = new List<ItemResponse>();
        var act = async () => items = await _itemService.GetTopItemsByScoreAsync(2);
        
        //assert
        using (new AssertionScope())
        {
            await act.Should().NotThrowAsync();
            items.Count.Should().Be(2);
            items.Select(x => x.Id).Should().BeEquivalentTo(new[] { item1Id, item2Id });
        }
    }
}