using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using Microsoft.AspNetCore.Authorization;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [Authorize]
    [Route("api/plant-types")]
    [ApiController]
    public class PlantTypesController : ControllerBase
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public PlantTypesController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetPlantTypes()
        {
            var plantTypes = await _context.PlantType
                .Select(pt => new
                {
                    plantTypeId = pt.PlantTypeId,
                    plantTypeName = pt.PlantTypeName
                })
                .OrderBy(pt => pt.plantTypeName)
                .ToListAsync();

            return Ok(plantTypes);
        }
    }
}
