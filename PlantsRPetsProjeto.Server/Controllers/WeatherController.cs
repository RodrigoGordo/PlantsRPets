using Microsoft.AspNetCore.Mvc;
using PlantsRPetsProjeto.Server.Services;
using System.Threading.Tasks;

namespace PlantsRPetsProjeto.Server.Controllers
{
    /// <summary>
    /// Controlador responsável por obter dados meteorológicos através de uma cidade ou coordenadas geográficas.
    /// </summary>
    [Route("api/weather")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        /// <summary>
        /// Construtor do controlador meteorológico.
        /// </summary>
        /// <param name="weatherService">Serviço responsável por consultar os dados meteorológicos.</param>
        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        /// <summary>
        /// Devolve os dados meteorológicos atuais para a cidade especificada.
        /// </summary>
        /// <param name="city">Nome da cidade.</param>
        /// <returns>Dados meteorológicos ou 404 caso não sejam encontrados.</returns>
        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetWeatherByCity(string city)
        {
            var weatherData = await _weatherService.GetWeatherAsync(city);
            if (weatherData == null) return NotFound();
            return Ok(weatherData);
        }

        /// <summary>
        /// Devolve os dados meteorológicos atuais para as coordenadas fornecidas.
        /// </summary>
        /// <param name="lat">Latitude do local.</param>
        /// <param name="lon">Longitude do local.</param>
        /// <returns>Dados meteorológicos ou 404 caso não sejam encontrados.</returns>
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
