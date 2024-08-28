using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;
using APIAggregationService.Controllers;
using APIAggregationService.Models.Dtos;
using APIAggregationService.Services.OpenWeatherMap;

public class SpotifyControllerTests
{
    private readonly Mock<ISpotifyService> _spotifyServiceMock;
    private readonly Mock<ILogger<SpotifyController>> _loggerMock;
    private readonly SpotifyController _controller;

    public SpotifyControllerTests()
    {
        _spotifyServiceMock = new Mock<ISpotifyService>();
        _loggerMock = new Mock<ILogger<SpotifyController>>();
        _controller = new SpotifyController(_spotifyServiceMock.Object);
    }

    [Fact]
    public async Task GetTrackDetails_ReturnsOkResult_WhenServiceReturnsData()
    {
        // Arrange
        var trackId = "track-id";
        var trackDto = new TrackDto
        {
            Name = "Track Name",
            Album = "Album Name",
            Artists = new[] { "Artist Name" },
            DurationMs = 180000,
            Popularity = 80,
            ReleaseDate = "2022-01-01",
            PreviewUrl = "https://preview.url"
        };

        _spotifyServiceMock.Setup(service => service.GetTrackDetailsAsync(trackId))
            .ReturnsAsync(trackDto);

        // Act
        var result = await _controller.GetTrackDetails(trackId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTrackDto = Assert.IsType<TrackDto>(okResult.Value);
        Assert.Equal(trackDto.Name, returnedTrackDto.Name);
    }

    [Fact]
    public async Task GetTrackDetails_ReturnsInternalServerError_WhenServiceThrowsException()
    {
        // Arrange
        var trackId = "track-id";

        _spotifyServiceMock.Setup(service => service.GetTrackDetailsAsync(trackId))
            .ThrowsAsync(new Exception("Service error"));

        // Act
        var result = await _controller.GetTrackDetails(trackId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Unable to retrieve track information.", ((dynamic)objectResult.Value).error);
    }

    [Fact]
    public async Task GetAlbumDetails_ReturnsOkResult_WhenServiceReturnsData()
    {
        // Arrange
        var albumId = "album-id";
        var albumDto = new AlbumDto
        {
            Name = "Album Name",
            ReleaseDate = "2022-01-01",
            Artists = new[] { "Artist Name" },
            Tracks = new List<TrackDto>
            {
                new TrackDto { Name = "Track 1" }
            }
        };

        _spotifyServiceMock.Setup(service => service.GetAlbumDetailsAsync(albumId))
            .ReturnsAsync(albumDto);

        // Act
        var result = await _controller.GetAlbumDetails(albumId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedAlbumDto = Assert.IsType<AlbumDto>(okResult.Value);
        Assert.Equal(albumDto.Name, returnedAlbumDto.Name);
    }

    [Fact]
    public async Task GetAlbumDetails_ReturnsInternalServerError_WhenServiceThrowsException()
    {
        // Arrange
        var albumId = "album-id";

        _spotifyServiceMock.Setup(service => service.GetAlbumDetailsAsync(albumId))
            .ThrowsAsync(new Exception("Service error"));

        // Act
        var result = await _controller.GetAlbumDetails(albumId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Unable to retrieve album information.", ((dynamic)objectResult.Value).error);
    }

    [Fact]
    public async Task GetPlaylistTracks_ReturnsOkResult_WhenServiceReturnsData()
    {
        // Arrange
        var playlistId = "playlist-id";
        var tracks = new List<TrackDto>
        {
            new TrackDto { Name = "Track 1" }
        };

        _spotifyServiceMock.Setup(service => service.GetPlaylistTracksAsync(playlistId))
            .ReturnsAsync(tracks);

        // Act
        var result = await _controller.GetPlaylistTracks(playlistId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTracks = Assert.IsType<List<TrackDto>>(okResult.Value);
        Assert.Single(returnedTracks, track => track.Name == "Track 1");
    }

    [Fact]
    public async Task GetPlaylistTracks_ReturnsInternalServerError_WhenServiceThrowsException()
    {
        // Arrange
        var playlistId = "playlist-id";

        _spotifyServiceMock.Setup(service => service.GetPlaylistTracksAsync(playlistId))
            .ThrowsAsync(new Exception("Service error"));

        // Act
        var result = await _controller.GetPlaylistTracks(playlistId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Unable to retrieve playlist information.", ((dynamic)objectResult.Value).error);
    }
}
