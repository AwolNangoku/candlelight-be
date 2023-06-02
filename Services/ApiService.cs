using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CandleLightApi.Services
{
    public class ApiService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ApiService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }


        public async Task<string> GetAsync(string baseUrl, Dictionary<string, string> queryParameters)
        {
            var queryString = string.Join("&", queryParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            var requestUrl = $"{baseUrl}?{queryString}";

            _httpClient.DefaultRequestHeaders.Clear();

            var response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return responseData;
            }

            throw new Exception($"Failed to retrieve data. Status code: {response.StatusCode}");
        }



        private string ToQueryString(Dictionary<string, string> queryParameters)
        {
            var queryStrings = queryParameters
                .Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}");

            return string.Join("&", queryStrings);
        }


        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest requestData,string baseUrl, string appId, string appKey)
        {
            

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("app_id", appId);
            _httpClient.DefaultRequestHeaders.Add("app_key", appKey);

            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/{endpoint}", requestData);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<TResponse>();
                return responseData;
            }

            throw new Exception($"Failed to post data. Status code: {response.StatusCode}");
        }
    }
}
