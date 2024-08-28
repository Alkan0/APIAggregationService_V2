using APIAggregationService.Models;

namespace APIAggregationService.Services.Cat
{
    public interface IFavoriteService
    {
        Task<Favorite> AddFavoriteAsync(string imageId, string userId);
        Task RemoveFavoriteAsync(string favoriteId);

    }
}
