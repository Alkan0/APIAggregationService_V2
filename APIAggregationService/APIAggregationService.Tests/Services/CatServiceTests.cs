using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using APIAggregationService.Services.Cat;
using APIAggregationService.Models.Dtos;

public class CatServiceTests
{
    [Fact]
    public async Task GetCatsAsync_ShouldReturnListOfCats()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new List<CatDto>
                {
                    new CatDto { Id = "1", Url = "https://example.com/cat1.jpg" },
                    new CatDto { Id = "2", Url = "https://example.com/cat2.jpg" }
                }))
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var catService = new CatService(httpClient);

        // Act
        var result = await catService.GetCatsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("1", result[0].Id);
    }
}
