using APIAggregationService.Models.Dtos;

public interface ISpotifyService
{
    Task<TrackDto> GetTrackDetailsAsync(string trackId);
    Task<string> GetAccessTokenAsync();
    Task<List<TrackDto>> GetPlaylistTracksAsync(string playlistId);
    Task<AlbumDto> GetAlbumDetailsAsync(string albumId);
}