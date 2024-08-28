using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

public class VoteService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "live_tm3klEQzLeHeVhNvd2bSNZxJJsJpFirmRRrZugOpbRbxqYrmTlhB2wtgNvFf6cHN";
    private readonly string _baseUri = "https://api.thecatapi.com/v1/";

    public VoteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
    }

    public async Task VoteForCatAsync(string imageId, int value, string subId)
    {
        var content = new StringContent(JsonConvert.SerializeObject(new { image_id = imageId, value, sub_id = subId }), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_baseUri + "votes", content);
        response.EnsureSuccessStatusCode();
    }
}
