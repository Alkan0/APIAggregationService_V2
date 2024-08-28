using APIAggregationService.Models;
using APIAggregationService.Models.Dtos;

namespace APIAggregationService.Services.Cat
{
    public interface ICatService
    {
        Task<List<CatDto>> GetCatsAsync();
    }
}