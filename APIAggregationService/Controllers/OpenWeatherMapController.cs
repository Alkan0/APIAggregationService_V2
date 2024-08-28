using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using APIAggregationService.Services;
using APIAggregationService.Models;
using APIAggregationService.Services.OpenWeatherMap;

namespace APIAggregationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenWeatherMapController : ControllerBase
    {
        private readonly IOpenWeatherMapService _service;
        private readonly ILogger<OpenWeatherMapController> _logger;

        public OpenWeatherMapController(IOpenWeatherMapService externalApiService, ILogger<OpenWeatherMapController> logger)
        {
            _service = externalApiService;
            _logger = logger;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            try
            {
                var weatherDto = await _service.GetWeatherAsync(city);
                return Ok(weatherDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching weather data for city {City}.", city);
                return StatusCode(500, new { error = "Unable to retrieve weather data." });
            }
        }
    }
}
