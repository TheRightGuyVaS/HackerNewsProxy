using AutoMapper;
using HackerNewsProxy.Api.Models;
using Microsoft.AspNetCore.Mvc;
using HackerNewsProxy.Business.Interfaces;

namespace HackerNewsProxy.Api.Controllers;

[Route("stories")]
public class StoriesController : Controller
{
    private readonly IItemService _itemService;
    private readonly IMapper _mapper;

    public StoriesController(IItemService itemService, IMapper mapper)
    {
        _itemService = itemService;
        _mapper = mapper;
    }

    [HttpGet("best/{amount:int}")]
    [ProducesResponseType(typeof(ItemModel[]), 200)]
    public async Task<JsonResult> GetBestStories(int amount)
    {
        var stories = await this._itemService.GetTopItemsByScoreAsync(amount);
        return Json(_mapper.Map<ItemModel[]>(stories));
    }
}