using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using APIAggregationService.Models;

public class FavoriteService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "YOUR_API_KEY";
    private readonly string _baseUri = "https://api.thecatapi.com/v1/";

    public FavoriteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
    }

    public async Task<Favorite> AddFavoriteAsync(string imageId, string userId)
    {
        var content = new StringContent(JsonConvert.SerializeObject(new { image_id = imageId, sub_id = userId }), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_baseUri + "favourites", content);
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<Favorite>(await response.Content.ReadAsStringAsync());
    }

    public async Task RemoveFavoriteAsync(string favoriteId)
    {
        await _httpClient.DeleteAsync(_baseUri + $"favourites/{favoriteId}");
    }
}
