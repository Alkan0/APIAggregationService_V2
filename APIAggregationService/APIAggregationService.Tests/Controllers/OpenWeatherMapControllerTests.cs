using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using APIAggregationService.Controllers;
using APIAggregationService.Services.OpenWeatherMap;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using APIAggregationService.Models.Dtos;
using System;

public class OpenWeatherMapControllerTests
{
    [Fact]
    public async Task GetWeather_ReturnsOkResult_WhenServiceReturnsData()
    {
        // Arrange
        var city = "TestCity";
        var weatherDto = new WeatherDto
        {
            City = city,
            Temperature = 25.0,
            Humidity = 60,
            WindSpeed = 5.0,
            Pressure = 1015,
            WeatherDescription = "clear sky"
        };

        var serviceMock = new Mock<IOpenWeatherMapService>();
        serviceMock.Setup(service => service.GetWeatherAsync(city))
            .ReturnsAsync(weatherDto);

        var loggerMock = new Mock<ILogger<OpenWeatherMapController>>();
        var controller = new OpenWeatherMapController(serviceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetWeather(city);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsType<WeatherDto>(okResult.Value);
        Assert.Equal(weatherDto.City, returnedDto.City);
        Assert.Equal(weatherDto.Temperature, returnedDto.Temperature);
    }

    [Fact]
    public async Task GetWeather_ReturnsInternalServerError_WhenServiceThrowsException()
    {
        // Arrange
        var city = "TestCity";

        var serviceMock = new Mock<IOpenWeatherMapService>();
        serviceMock.Setup(service => service.GetWeatherAsync(city))
            .ThrowsAsync(new Exception("Service error"));

        var loggerMock = new Mock<ILogger<OpenWeatherMapController>>();
        var controller = new OpenWeatherMapController(serviceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetWeather(city);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Unable to retrieve weather data.", ((dynamic)objectResult.Value).error);
    }
}
