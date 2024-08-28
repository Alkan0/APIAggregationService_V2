using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using APIAggregationService.Models;

namespace APIAggregationService.APIAggregationService.Tests
{
    public class BreedServiceTests
    {
        [Fact]
        public async Task GetBreedsAsync_ShouldReturnBreedsFromCacheIfAvailable()
        {
            // Arrange
            var mockCache = new Mock<IMemoryCache>();
            object cachedBreeds = new List<Breed> { new Breed { Id = "1", Name = "Breed1" } };
            mockCache.Setup(x => x.TryGetValue("breeds", out cachedBreeds)).Returns(true);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var breedService = new BreedService(httpClient, mockCache.Object);

            // Act
            var result = await breedService.GetBreedsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Breed1", result[0].Name);
        }

        [Fact]
        public async Task GetBreedsAsync_ShouldFetchBreedsIfNotCached()
        {
            // Arrange
            var mockCache = new Mock<IMemoryCache>();
            object cachedBreeds;
            mockCache.Setup(x => x.TryGetValue("breeds", out cachedBreeds)).Returns(false);

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
                    Content = new StringContent(JsonConvert.SerializeObject(new List<Breed>
                    {
                    new Breed { Id = "1", Name = "Breed1" }
                    }))
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var breedService = new BreedService(httpClient, mockCache.Object);

            // Act
            var result = await breedService.GetBreedsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Breed1", result[0].Name);
        }
    }
}