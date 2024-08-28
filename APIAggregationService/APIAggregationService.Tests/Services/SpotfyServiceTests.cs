using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using APIAggregationService.Models.Dtos;
using APIAggregationService.Services;
using APIAggregationService.Models;

public class SpotifyServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly SpotifyService _spotifyService;

    public SpotifyServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _spotifyService = new SpotifyService(new ConfigurationBuilder().Build());
    }

    [Fact]
    public async Task GetAccessTokenAsync_ReturnsToken_WhenResponseIsSuccessful()
    {
        // Arrange
        var jsonResponse = JsonSerializer.Serialize(new SpotifyTokenResponse { access_token = "test-token" });
        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes("client-id:client-secret"));

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new System.Uri("https://accounts.spotify.com/api/token") &&
                    req.Headers.Authorization.Parameter == authToken),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            });

        // Act
        var token = await _spotifyService.GetAccessTokenAsync();

        // Assert
        Assert.Equal("test-token", token);
    }

    [Fact]
    public async Task GetTrackDetailsAsync_ReturnsTrackDto_WhenResponseIsSuccessful()
    {
        // Arrange
        var trackId = "track-id";
        var trackJson = JsonSerializer.Serialize(new TrackDto
        {
            Name = "Track Name",
            Album = "Album Name",
            Artists = new[] { "Artist Name" },
            DurationMs = 180000,
            Popularity = 80,
            ReleaseDate = "2022-01-01",
            PreviewUrl = "https://preview.url"
        });

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new System.Uri($"https://api.spotify.com/v1/tracks/{trackId}")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(trackJson, Encoding.UTF8, "application/json")
            });

        // Act
        var trackDto = await _spotifyService.GetTrackDetailsAsync(trackId);

        // Assert
        Assert.Equal("Track Name", trackDto.Name);
        Assert.Equal("Album Name", trackDto.Album);
        Assert.Single(trackDto.Artists, "Artist Name");
    }

    [Fact]
    public async Task GetPlaylistTracksAsync_ReturnsListOfTrackDtos_WhenResponseIsSuccessful()
    {
        // Arrange
        var playlistId = "playlist-id";
        var tracksJson = JsonSerializer.Serialize(new
        {
            items = new[]
            {
                new
                {
                    track = new TrackDto
                    {
                        Name = "Track 1",
                        Album = "Album 1",
                        Artists = new[] { "Artist 1" },
                        DurationMs = 200000,
                        Popularity = 90,
                        ReleaseDate = "2022-01-01",
                        PreviewUrl = "https://preview.url/1"
                    }
                },
                new
                {
                    track = new TrackDto
                    {
                        Name = "Track 2",
                        Album = "Album 2",
                        Artists = new[] { "Artist 2" },
                        DurationMs = 210000,
                        Popularity = 85,
                        ReleaseDate = "2022-02-01",
                        PreviewUrl = "https://preview.url/2"
                    }
                }
            }
        });

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new System.Uri($"https://api.spotify.com/v1/playlists/{playlistId}/tracks")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(tracksJson, Encoding.UTF8, "application/json")
            });

        // Act
        var tracks = await _spotifyService.GetPlaylistTracksAsync(playlistId);

        // Assert
        Assert.Equal(2, tracks.Count);
        Assert.Contains(tracks, track => track.Name == "Track 1");
    }

    [Fact]
    public async Task GetAlbumDetailsAsync_ReturnsAlbumDto_WhenResponseIsSuccessful()
    {
        // Arrange
        var albumId = "album-id";
        var albumJson = JsonSerializer.Serialize(new
        {
            name = "Album Name",
            release_date = "2022-01-01",
            artists = new[] { new { name = "Artist Name" } },
            tracks = new
            {
                items = new[]
                {
                    new TrackDto
                    {
                        Name = "Track 1",
                        DurationMs = 200000,
                        Artists = new[] { "Artist 1" },
                        ReleaseDate = "2022-01-01",
                        PreviewUrl = "https://preview.url/1"
                    }
                }
            }
        });

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new System.Uri($"https://api.spotify.com/v1/albums/{albumId}")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(albumJson, Encoding.UTF8, "application/json")
            });

        // Act
        var albumDto = await _spotifyService.GetAlbumDetailsAsync(albumId);

        // Assert
        Assert.Equal("Album Name", albumDto.Name);
        Assert.Single(albumDto.Tracks, track => track.Name == "Track 1");
    }
}
