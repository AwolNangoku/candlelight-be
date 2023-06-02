using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandleLightApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CandleLightApi.Controllers
{
    [Route("api/[controller]")]
    public class Food : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly ApiService _apiService;
        public Food(IConfiguration configuration,ApiService apiService)
        {
            _configuration = configuration;
            _apiService = apiService;
        }
    

        // GET api/values/5
        [HttpGet("{foodName}")]
        public  string   Get(string foodName)
        {
            var baseUrl = _configuration["AppSettings:FoodApiBaseUrl"];
            var appId = _configuration["AppSettings:app_id"];
            var appKey = _configuration["AppSettings:app_key"];
            var endpoint = baseUrl;
            // Build the query parameters
            
            var queryParameters = new Dictionary<string, string>
            {
                { "app_id", appId },
                { "app_key", appKey },
                { "ingr",  foodName},
                { "nutrition-type", "cooking" }
            };

            var responseData =   _apiService.GetAsync(baseUrl, queryParameters).Result;


            return responseData;

        }

       
    }
}

