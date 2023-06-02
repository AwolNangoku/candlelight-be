using CandleLightApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CandleLightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadSheddingController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IConfiguration _config;
        private const string _apiKey = "E75336D6-6DC6424A-817DD807-79F7D491";

/// <summary>
/// Get LoadShedding Status Nationally
/// </summary>
/// <returns></returns>
/// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<ActionResult<LoadShedStatus>> GetStatus()
        {
            client.DefaultRequestHeaders.Add("Token", _apiKey);
            var apiUrl = "https://developer.sepush.co.za/business/2.0/status";
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                LoadShedStatus loadShedStatus = JsonConvert.DeserializeObject<LoadShedStatus>(responseBody);
                return Ok(loadShedStatus);
            }
            else
            {
                throw new Exception("API request failed with status code: " + response.StatusCode);
            }
        }

        /// <summary>
        /// Get loadshedding schedule for a specific area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{area}")]
        public async Task<ActionResult<ScheduleRoot>> GetSchedule(string area)
        {
            client.DefaultRequestHeaders.Add("Token", _apiKey);
            var apiUrl = "https://developer.sepush.co.za/business/2.0/area?id=" + area;
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                ScheduleRoot schedule = JsonConvert.DeserializeObject<ScheduleRoot>(responseBody);
                return Ok(schedule);
            }
            else
            {
                throw new Exception("API request failed with status code: " + response.StatusCode);
            }
        }

        /// <summary>
        /// Search area based on text
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
      /*  [HttpGet("search/{area}")]
        public async Task<ActionResult<AreaRoot>> GetAreas(string area)
        {
            client.DefaultRequestHeaders.Add("Token", _apiKey);
            var apiUrl = "https://developer.sepush.co.za/business/2.0/areas_search?text=" + area;
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                AreaRoot areas = JsonConvert.DeserializeObject<AreaRoot>(responseBody);

                return Ok(areas);
            }
            else
            {
                throw new Exception("API request failed with status code: " + response.StatusCode);
            }
        } */

        /// <summary>
        /// Find areas based on GPS coordinates (latitude and longitude).
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{lat}/{lon}")]
        public async Task<ActionResult<NearRoot>> GetNearAreas(string lat, string lon)
        {
            client.DefaultRequestHeaders.Add("Token", _apiKey);
            var BaseUrl = "https://developer.sepush.co.za/business/2.0/";
            string apiUrl = $"{BaseUrl}areas_nearby?lat={lat}&lon={lon}";
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                NearRoot nearby = JsonConvert.DeserializeObject<NearRoot>(responseBody);
                return Ok(nearby);
            }
            else
            {
                throw new Exception("API request failed with status code: " + response.StatusCode);
            }
        }

    }
}
