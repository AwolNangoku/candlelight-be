using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandleLightApi.Services;
using Microsoft.AspNetCore.Mvc;
using CandleLightApi.Models;
using static CandleLightApi.Models.openApi;
using Newtonsoft.Json;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CandleLightApi.Controllers
{
    [Route("api/[controller]")]
    public class RecipeController : Controller
    {

        private readonly IConfiguration _configuration;

        private readonly ApiService _apiService;

        public RecipeController(IConfiguration configuration, ApiService apiService)
        {
            _configuration = configuration;
            _apiService = apiService;
        }

        [HttpPost]
        [Route("ingrideints")]
        public async Task<IActionResult> GenerateRecipesAsync([FromBody] List<string> ingrideints)
        {
            try
            {
                var baseUrl = _configuration["AppSettings:OpenAiApiUrl"];
                var bearerToken = _configuration["AppSettings:OpenAIBearearTokeen"];

                var responseData = await _apiService.PostAsync(baseUrl,  bearerToken, ingrideints);

                JsonConvert.SerializeObject(responseData);

                // Process the response data as needed

            return Ok(responseData);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}

