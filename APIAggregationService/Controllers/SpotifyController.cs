using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using APIAggregationService.Models.Dtos;

namespace APIAggregationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotifyController : ControllerBase
    {
        private readonly ISpotifyService _spotifyService;

        public SpotifyController(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        // Endpoint to get track details
        [HttpGet("track/{trackId}")]
        public async Task<IActionResult> GetTrackDetails(string trackId)
        {
            try
            {
                var trackDetails = await _spotifyService.GetTrackDetailsAsync(trackId);
                return Ok(trackDetails);
            }
            catch
            {
                return StatusCode(500, new { error = "Unable to retrieve track information." });
            }
        }

        // Endpoint to get album details
        [HttpGet("album/{albumId}")]
        public async Task<IActionResult> GetAlbumDetails(string albumId)
        {
            try
            {
                var albumDetails = await _spotifyService.GetAlbumDetailsAsync(albumId);
                return Ok(albumDetails);
            }
            catch
            {
                return StatusCode(500, new { error = "Unable to retrieve album information." });
            }
        }

        [HttpGet("playlist/{playlistId}")]
        public async Task<IActionResult> GetPlaylistTracks(string playlistId)
        {
            try
            {
                var playlistTracks = await _spotifyService.GetPlaylistTracksAsync(playlistId);
                return Ok(playlistTracks);
            }
            catch
            {
                return StatusCode(500, new { error = "Unable to retrieve playlist information." });
            }
        }
    }
}
