using Microsoft.AspNetCore.Mvc;
using PlantsRPetsProjeto.Server.Services;
using System.Threading.Tasks;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [Route("api/weather")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetWeatherByCity(string city)
        {
            var weatherData = await _weatherService.GetWeatherAsync(city);
            if (weatherData == null) return NotFound();
            return Ok(weatherData);
        }

        [HttpGet("coords/{lat}/{lon}")]
        public async Task<IActionResult> GetWeatherByCoords(double lat, double lon)
        {
            var location = $"{lat},{lon}";
            var weatherData = await _weatherService.GetWeatherAsync(location);
            if (weatherData == null) return NotFound();
            return Ok(weatherData);
        }
    }
}
