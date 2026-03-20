using HotelMind.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelMind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        // FIXED: The constructor was missing!
        public WeatherController(IHttpClientFactory httpClientFactory, EnvConfig config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiKey = config.WEATHER_API_KEY;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            Console.WriteLine($"\n--- [DEBUG] Weather Request for: {city} ---");

            if (string.IsNullOrEmpty(_apiKey))
            {
                Console.WriteLine("--- [ERROR] Weather API Key is missing! Check your .env or EnvConfig. ---");
                return BadRequest("API Key missing");
            }

            var url = $"https://api.weatherapi.com/v1/forecast.json?key={_apiKey}&q={city}&days=7&aqi=no&alerts=no";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"--- [SUCCESS] Weather data received for {city} ---");
                    return Content(content, "application/json");
                }

                Console.WriteLine($"--- [ERROR] API returned {response.StatusCode} for city: {city} ---");
                return StatusCode((int)response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--- [EXCEPTION] {ex.Message} ---");
                return StatusCode(500);
            }
        }
    }
}