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

        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            var weatherData = await _weatherService.GetWeatherAsync(city);
            if (weatherData == null) return NotFound();
            return Ok(weatherData);
        }

    }
}
