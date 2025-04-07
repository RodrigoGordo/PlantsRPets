using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using Microsoft.AspNetCore.Authorization;

namespace PlantsRPetsProjeto.Server.Controllers
{
    /// <summary>
    /// Controlador responsável pela gestão e consulta dos tipos de planta disponíveis na aplicação.
    /// </summary>
    [Authorize]
    [Route("api/plant-types")]
    [ApiController]
    public class PlantTypesController : ControllerBase
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        /// <summary>
        /// Construtor do controlador de tipos de planta.
        /// </summary>
        /// <param name="context">Contexto da base de dados da aplicação.</param>
        public PlantTypesController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devolve a lista de todos os tipos de planta registados na aplicação.
        /// </summary>
        /// <returns>Lista de objetos com o identificador e nome de cada tipo de planta.</returns>
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
