using APIAggregationService.Models;
using APIAggregationService.Models.Dtos;
using APIAggregationService.Services.Cat;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CatsController : ControllerBase
{
    private readonly CatService _catService;
    private readonly BreedService _breedService;
    private readonly FavoriteService _favoriteService;

    public CatsController(
        CatService catService,
        BreedService breedService,
        FavoriteService favoriteService,
        VoteService voteService)
    {
        _catService = catService;
        _breedService = breedService;
        _favoriteService = favoriteService;
    }

    // Fetch random cat images
    [HttpGet("images")]
    public async Task<ActionResult<List<CatDto>>> GetCats()
    {
        var cats = await _catService.GetCatsAsync();
        return Ok(cats);
    }

    // Fetch all breeds
    [HttpGet("breeds")]
    public async Task<ActionResult<List<Breed>>> GetBreeds()
    {
        var breeds = await _breedService.GetBreedsAsync();
        return Ok(breeds);
    }

    // Add a cat to favorites
    [HttpPost("favorites")]
    public async Task<ActionResult<Favorite>> AddFavorite([FromQuery] string imageId, [FromQuery] string userId)
    {
        var favorite = await _favoriteService.AddFavoriteAsync(imageId, userId);
        return Ok(favorite);
    }

    // Remove a favorite cat
    [HttpDelete("favorites/{favoriteId}")]
    public async Task<IActionResult> RemoveFavorite(string favoriteId)
    {
        await _favoriteService.RemoveFavoriteAsync(favoriteId);
        return NoContent();
    }

}
