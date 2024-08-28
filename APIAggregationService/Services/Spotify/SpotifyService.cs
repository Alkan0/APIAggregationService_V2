using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using APIAggregationService.Models;
using APIAggregationService.Models.Dtos;
using Microsoft.Extensions.Configuration;
using SpotifyAPI.Web;

public class SpotifyService : ISpotifyService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly HttpClient _httpClient;
    private SpotifyClient _spotifyClient;

    public SpotifyService(IConfiguration configuration)
    {
        _clientId = configuration["Spotify:ClientId"];      // Store the client ID in appsettings.json
        _clientSecret = configuration["Spotify:ClientSecret"]; // Store the client secret in appsettings.json
        _httpClient = new HttpClient();
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));

        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Failed to retrieve access token from Spotify.");
        }

        var responseString = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<SpotifyTokenResponse>(responseString);

        return tokenResponse.access_token;
    }

    public async Task<TrackDto> GetTrackDetailsAsync(string trackId)
    {
        // Fetch access token
        var accessToken = await GetAccessTokenAsync();

        // Use SpotifyClient with the access token
        _spotifyClient = new SpotifyClient(accessToken);

        var track = await _spotifyClient.Tracks.Get(trackId);
        var trackDto = new TrackDto
        {
            Name = track.Name,
            Album = track.Album.Name,
            Artists = track.Artists.Select(artist => artist.Name).ToArray(),
            DurationMs = track.DurationMs,
            Popularity = track.Popularity,
            ReleaseDate = track.Album.ReleaseDate,
            PreviewUrl = track.PreviewUrl
        };
        return trackDto;
    }

    public async Task<List<TrackDto>> GetPlaylistTracksAsync(string playlistId)
    {
        var accessToken = await GetAccessTokenAsync();
        _spotifyClient = new SpotifyClient(accessToken);

        var playlist = await _spotifyClient.Playlists.GetItems(playlistId);
        var tracks = playlist.Items
            .Where(item => item.Track is FullTrack)
            .Select(item => item.Track as FullTrack)
            .Select(track => new TrackDto
            {
                Name = track.Name,
                Album = track.Album.Name,
                Artists = track.Artists.Select(artist => artist.Name).ToArray(),
                DurationMs = track.DurationMs,
                Popularity = track.Popularity,
                ReleaseDate = track.Album.ReleaseDate,
                PreviewUrl = track.PreviewUrl
            }).ToList();

        return tracks;
    }

    public async Task<AlbumDto> GetAlbumDetailsAsync(string albumId)
    {
        var accessToken = await GetAccessTokenAsync();
        _spotifyClient = new SpotifyClient(accessToken);

        // Fetch album details from Spotify
        var album = await _spotifyClient.Albums.Get(albumId);

        // Convert album tracks to TrackDto format
        var tracks = album.Tracks.Items.Select(track => new TrackDto
        {
            Name = track.Name,
            DurationMs = track.DurationMs,
            Artists = track.Artists.Select(artist => artist.Name).ToArray(),
            Popularity = 0, // Spotify Album API doesn't return popularity for individual tracks
            ReleaseDate = album.ReleaseDate,
            PreviewUrl = track.PreviewUrl
        }).ToList();

        // Create AlbumDto with detailed information
        var albumDto = new AlbumDto
        {
            Name = album.Name,
            ReleaseDate = album.ReleaseDate,
            Artists = album.Artists.Select(artist => artist.Name).ToArray(),
            Tracks = tracks
        };

        return albumDto;
    }
}
