// Services/ExternalApiService.cs
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using APIAggregationService.Models;
using Microsoft.Extensions.Logging;
using APIAggregationService.Models.Dtos;
using APIAggregationService.Services.OpenWeatherMap;

public class OpenWeatherMapService : IOpenWeatherMapService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenWeatherMapService> _logger;
    private const string ApiKey = "your-api-key"; 

    public OpenWeatherMapService(HttpClient httpClient, ILogger<OpenWeatherMapService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<WeatherDto> GetWeatherAsync(string city)
    {
        var requestUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={ApiKey}&units=metric";

        try
        {
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);

            var temperature = json["main"]["temp"].Value<double>();
            var humidity = json["main"]["humidity"].Value<int>();
            var windSpeed = json["wind"]["speed"].Value<double>();
            var pressure = json["main"]["pressure"].Value<double>();
            var weatherDescription = json["weather"][0]["description"].Value<string>();

            return new WeatherDto
            {
                City = city,
                Temperature = temperature,
                Humidity = humidity,
                WindSpeed = windSpeed,
                Pressure = pressure,
                WeatherDescription = weatherDescription
            };
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "Error fetching weather data from OpenWeatherMap for city {City}.", city);
            throw;
        }
    }
}
