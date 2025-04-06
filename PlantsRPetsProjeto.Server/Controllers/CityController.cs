using Microsoft.AspNetCore.Mvc;
using PlantsRPetsProjeto.Server.Services;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly CityService _cityService;

        public CityController(CityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet("search/{name}")]
        public async Task<IActionResult> GetCitiesByQuery(string name)
        {
            var citiesData = await _cityService.GetCitiesWithNameAsync(name);
            if (citiesData == null) return NotFound();
            return Ok(citiesData);
        }
    }
}