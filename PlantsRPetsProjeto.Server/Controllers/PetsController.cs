using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Controlador responsável pela consulta da lista de pets disponíveis na aplicação.
    /// </summary>
    public class PetsController : Controller
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        /// <summary>
        /// Construtor do controlador de pets.
        /// </summary>
        /// <param name="context">Contexto da base de dados da aplicação.</param>
        public PetsController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devolve a lista completa de pets disponíveis.
        /// </summary>
        /// <returns>Lista de objetos do tipo <see cref="Pet"/>.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPets()
        {
            return await _context.Pet.ToListAsync();
        }

        /// <summary>
        /// Devolve os dados de um pet específico com base no seu identificador.
        /// </summary>
        /// <param name="id">Identificador do pet.</param>
        /// <returns>Objeto do tipo <see cref="Pet"/> ou erro se não for encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            var pet = await _context.Pet.FindAsync(id);

            if (pet == null)
            {
                return NotFound();
            }

            return pet;
        }

    }
}
