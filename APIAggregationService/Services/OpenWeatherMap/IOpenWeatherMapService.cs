using APIAggregationService.Models.Dtos;

namespace APIAggregationService.Services.OpenWeatherMap
{
    public interface IOpenWeatherMapService
    {
        Task<WeatherDto> GetWeatherAsync(string city);
    }
}
