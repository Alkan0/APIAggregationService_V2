using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using APIAggregationService.Models;
using APIAggregationService.Models.Dtos;
using Newtonsoft.Json;

namespace APIAggregationService.Services.Cat
{
    public class CatService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "live_tm3klEQzLeHeVhNvd2bSNZxJJsJpFirmRRrZugOpbRbxqYrmTlhB2wtgNvFf6cHN"; 
        private readonly string _baseUri = "https://api.thecatapi.com/v1/images/search?limit=10";

        public CatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
        }

        public async Task<List<CatDto>> GetCatsAsync()
        {
            var response = await _httpClient.GetStringAsync(_baseUri + "images/search");
            return JsonConvert.DeserializeObject<List<CatDto>>(response);
        }

    }
}