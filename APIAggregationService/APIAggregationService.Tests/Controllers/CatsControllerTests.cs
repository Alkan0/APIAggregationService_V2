using APIAggregationService.Models;
using APIAggregationService.Models.Dtos;
using APIAggregationService.Services.Cat;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class CatsControllerTests
{
    [Fact]
    public async Task GetCats_ShouldReturnOkWithCats()
    {
        // Arrange
        var mockCatService = new Mock<CatService>(null);
        mockCatService.Setup(x => x.GetCatsAsync()).ReturnsAsync(new List<CatDto>
        {
            new CatDto { Id = "1", Url = "https://example.com/cat1.jpg" }
        });

        var controller = new CatsController(mockCatService.Object, null, null, null);

        // Act
        var result = await controller.GetCats();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var cats = Assert.IsType<List<CatDto>>(okResult.Value);
        Assert.Single(cats);
        Assert.Equal("1", cats[0].Id);
    }

    [Fact]
    public async Task GetBreeds_ShouldReturnOkWithBreeds()
    {
        // Arrange
        var mockBreedService = new Mock<BreedService>(null, null);
        mockBreedService.Setup(x => x.GetBreedsAsync()).ReturnsAsync(new List<Breed>
        {
            new Breed { Id = "1", Name = "Breed1" }
        });

        var controller = new CatsController(null, mockBreedService.Object, null, null);

        // Act
        var result = await controller.GetBreeds();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var breeds = Assert.IsType<List<Breed>>(okResult.Value);
        Assert.Single(breeds);
        Assert.Equal("Breed1", breeds[0].Name);
    }
}
