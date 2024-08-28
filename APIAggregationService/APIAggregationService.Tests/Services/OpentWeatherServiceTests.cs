using Xunit;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using APIAggregationService.Services.OpenWeatherMap;
using Microsoft.Extensions.Logging;
using System.Net;
using Moq.Protected;
using System.Threading;
using APIAggregationService.Models.Dtos;
using Newtonsoft.Json.Linq;
using System;

public class OpenWeatherMapServiceTests
{
    [Fact]
    public async Task GetWeatherAsync_ReturnsWeatherDto_WhenResponseIsSuccessful()
    {
        // Arrange
        var city = "TestCity";
        var jsonResponse = new JObject
        {
            ["main"] = new JObject
            {
                ["temp"] = 25.0,
                ["humidity"] = 60,
                ["pressure"] = 1015
            },
            ["wind"] = new JObject
            {
                ["speed"] = 5.0
            },
            ["weather"] = new JArray(
                new JObject
                {
                    ["description"] = "clear sky"
                }
            )
        }.ToString();

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var loggerMock = new Mock<ILogger<OpenWeatherMapService>>();
        var service = new OpenWeatherMapService(httpClient, loggerMock.Object);

        // Act
        var result = await service.GetWeatherAsync(city);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(25.0, result.Temperature);
        Assert.Equal(60, result.Humidity);
        Assert.Equal(5.0, result.WindSpeed);
        Assert.Equal(1015, result.Pressure);
        Assert.Equal("clear sky", result.WeatherDescription);
    }

    [Fact]
    public async Task GetWeatherAsync_ThrowsHttpRequestException_WhenResponseIsNotSuccessful()
    {
        // Arrange
        var city = "TestCity";
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var loggerMock = new Mock<ILogger<OpenWeatherMapService>>();
        var service = new OpenWeatherMapService(httpClient, loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => service.GetWeatherAsync(city));
    }
}
