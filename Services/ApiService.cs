using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CandleLightApi.Models.openApi;

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


        public async Task<object> PostAsync(string baseUrl, string bearerToken, List<string> ingrideients)
        {

            // Define the OpenAI API endpoint and your access token
            string apiUrl = "https://api.openai.com/v1/chat/completions";
            string accessToken = bearerToken;


            // Initialise the chat by describing the assistant,
            // and providing the assistants first question to the user
            var messages = new List<dynamic>
            {
                new {role = "system",
                    content = "You are an app for generating recipes based on ingridents " +
                                                "model trained by OpenAI. " +
                                                "You will recive a list of ingriedents and bring back mulple possible recipes you will say nothing else  " 
                                                 },
                new {role = "assistant",
                    content = "How can I help you?"}
            };
 

            while (true)
            {
                // Capture the users messages and add to
                // messages list for submitting to the chat API
                var userMessage =  string.Join(", ", ingrideients);
                messages.Add(new { role = "user", content = userMessage });

                // Create the request for the API sending the
                // latest collection of chat messages
                var request = new
                {
                    messages,
                    model = "gpt-3.5-turbo",
                    max_tokens = 300,
                };

                // Send the request and capture the response
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                var requestJson = JsonConvert.SerializeObject(request);
                var requestContent = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                var httpResponseMessage = await httpClient.PostAsync(apiUrl, requestContent);
                var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeAnonymousType(jsonString, new
                {
                    choices = new[] { new { message = new { role = string.Empty, content = string.Empty } } },
                    error = new { message = string.Empty }
                });


                return responseObject.choices;
            }
            // Process the response data as needed

         
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest requestData, string baseUrl, string appId, string appKey)
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

    public class OpenAIChatRequest
    {
        public string Model { get; set; }
        public List<ChatMessage> Messages { get; set; }
        public float Temperature { get; set; }
    }

    public class ChatMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

}
