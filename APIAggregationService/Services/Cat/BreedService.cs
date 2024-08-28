using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using APIAggregationService.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

public class BreedService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly string _apiKey = "live_tm3klEQzLeHeVhNvd2bSNZxJJsJpFirmRRrZugOpbRbxqYrmTlhB2wtgNvFf6cHN";
    private readonly string _baseUri = "https://api.thecatapi.com/v1/";

    public BreedService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
    }

    public BreedService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
        _cache = cache;
    }

    public async Task<List<Breed>> GetBreedsAsync()
    {
        var response = await _httpClient.GetStringAsync(_baseUri + "breeds");
        return JsonConvert.DeserializeObject<List<Breed>>(response);
    }
}
