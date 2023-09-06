using AutoMapper;
using HackerNewsProxy.Api.Models;
using Microsoft.AspNetCore.Mvc;
using HackerNewsProxy.Business.Interfaces;

namespace HackerNewsProxy.Api.Controllers;

[Route("stories")]
public class StoriesController : Controller
{
    private readonly IStoryService _storyService;
    private readonly IMapper _mapper;

    public StoriesController(IStoryService storyService, IMapper mapper)
    {
        _storyService = storyService;
        _mapper = mapper;
    }

    [HttpGet("best/{amount:int}")]
    [ProducesResponseType(typeof(ItemModel[]), 200)]
    public async Task<JsonResult> GetBestStories(int amount)
    {
        var stories = await this._storyService.GetTopStoriesAsync(amount);
        return Json(_mapper.Map<ItemModel[]>(stories));
    }
}