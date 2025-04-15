using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Controllers
{
    /// <summary>
    /// Controlador responsável pela gestão da coleção de pets de cada utilizador.
    /// Permite visualizar, adicionar, atualizar e obter pets favoritos ou ainda não adquiridos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollectionsController : Controller
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        /// <summary>
        /// Construtor do controlador de coleções.
        /// </summary>
        /// <param name="context">Contexto da base de dados da aplicação.</param>
        public CollectionsController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devolve a coleção de pets do utilizador autenticado, incluindo estado de posse e favoritos.
        /// Caso a coleção ainda não exista, é criada automaticamente.
        /// </summary>
        /// <returns>Lista de todos os pets, com marcações de posse e favoritos.</returns>
        [HttpGet]
        public async Task<ActionResult<object>> GetUserCollection()
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var collection = await _context.Collection
                .Include(c => c.CollectionPets)
                .ThenInclude(cp => cp.ReferencePet)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (collection == null)
            {
                collection = new Collection { UserId = userId, CollectionPets = new List<CollectionPets>() };
                _context.Collection.Add(collection);
                await _context.SaveChangesAsync();
            }

            var allPets = await _context.Pet.ToListAsync();

            var result = allPets.Select(pet => {
                var collectionPet = collection.CollectionPets.FirstOrDefault(cp => cp.PetId == pet.PetId);
                return new
                {
                    pet.PetId,
                    pet.Name,
                    pet.Type,
                    pet.Details,
                    pet.BattleStats,
                    pet.ImageUrl,
                    IsOwned = collectionPet != null && collectionPet.IsOwned,
                    IsFavorite = collectionPet != null && collectionPet.IsFavorite
                };
            });

            return Ok(result);
        }

        /// <summary>
        /// Adiciona um pet à coleção do utilizador, marcando-o como adquirido.
        /// </summary>
        /// <param name="petId">Identificador do pet a adicionar.</param>
        /// <returns>Mensagem de confirmação da adição.</returns>
        [HttpPost("add/{petId}")]
        public async Task<IActionResult> AddPetToCollection(int petId)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var pet = await _context.Pet.FindAsync(petId);
            if (pet == null)
            {
                return NotFound("Pet not found");
            }

            var collection = await _context.Collection
                .Include(c => c.CollectionPets)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (collection == null)
            {
                collection = new Collection { UserId = userId };
                _context.Collection.Add(collection);
                await _context.SaveChangesAsync();
            }

            var existingPet = collection.CollectionPets.FirstOrDefault(cp => cp.PetId == petId);
            if (existingPet != null)
            {
                existingPet.IsOwned = true;
            }
            else
            {
                collection.CollectionPets.Add(new CollectionPets
                {
                    PetId = petId,
                    IsOwned = true,
                    IsFavorite = false
                });
            }

            await _context.SaveChangesAsync();
            return Ok("Pet added to collection");
        }

        /// <summary>
        /// Alterna o estado de favorito de um pet que já foi adquirido.
        /// </summary>
        /// <param name="petId">Identificador do pet.</param>
        /// <returns>Estado atualizado do campo favorito.</returns>
        [HttpPut("favorite/{petId}")]
        public async Task<IActionResult> ToggleFavorite(int petId)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var collection = await _context.Collection
                .Include(c => c.CollectionPets)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (collection == null)
            {
                return NotFound("Collection not found");
            }

            var collectionPet = collection.CollectionPets.FirstOrDefault(cp => cp.PetId == petId);
            if (collectionPet == null || !collectionPet.IsOwned)
            {
                return BadRequest("You don't own this pet");
            }

            int favoriteCount = collection.CollectionPets.Count(cp => cp.IsFavorite);

            // Limit amount of favorite pets to 5
            if (!collectionPet.IsFavorite && favoriteCount >= 5)
            {
                return BadRequest("You can only have up to 5 favorite pets.");
            }

            collectionPet.IsFavorite = !collectionPet.IsFavorite;
            await _context.SaveChangesAsync();

            return Ok(new { isFavorite = collectionPet.IsFavorite });
        }

        /// <summary>
        /// Atualiza o estado de posse de um pet na coleção do utilizador.
        /// </summary>
        /// <param name="petId">Identificador do pet.</param>
        /// <param name="model">Objeto com o novo estado de posse.</param>
        /// <returns>Estado atualizado da posse do pet.</returns>
        [HttpPut("owned/{petId}")]
        public async Task<IActionResult> UpdateOwnedStatus(int petId, [FromBody] UpdateOwnedStatusModel model)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var pet = await _context.Pet.FindAsync(petId);
            if (pet == null)
            {
                return NotFound(new { message = "Pet not found." });
            }

            var collection = await _context.Collection
                .Include(c => c.CollectionPets)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (collection == null)
            {
                collection = new Collection { UserId = userId, CollectionPets = new List<CollectionPets>() };
                _context.Collection.Add(collection);
                await _context.SaveChangesAsync();
            }

            var collectionPet = collection.CollectionPets.FirstOrDefault(cp => cp.PetId == petId);

            if (collectionPet == null)
            {
                collectionPet = new CollectionPets
                {
                    PetId = petId,
                    IsOwned = model.IsOwned,
                    IsFavorite = false,
                    ReferenceCollection = collection
                };
                collection.CollectionPets.Add(collectionPet);
            }
            else
            {
                collectionPet.IsOwned = model.IsOwned;
            }

            await _context.SaveChangesAsync();

            return Ok(new { petId, isOwned = collectionPet.IsOwned });
        }

        /// <summary>
        /// Devolve uma lista aleatória de até 3 pets que o utilizador ainda não adquiriu.
        /// </summary>
        /// <returns>Lista com até 3 pets ainda não adquiridos.</returns>
        [HttpGet("random-unowned")]
        public async Task<IActionResult> GetRandomUnownedPets()
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var collection = await _context.Collection
                .Include(c => c.CollectionPets)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (collection == null)
            {
                collection = new Collection { UserId = userId, CollectionPets = new List<CollectionPets>() };
                _context.Collection.Add(collection);
                await _context.SaveChangesAsync();
            }

            var allPets = await _context.Pet.ToListAsync();

            var unownedPets = allPets
                .Where(pet =>
                {
                    var cp = collection.CollectionPets.FirstOrDefault(cp => cp.PetId == pet.PetId);
                    return cp == null || !cp.IsOwned;
                })
                .ToList();

            if (unownedPets.Count == 0)
            {
                return Ok(new List<Pet>());
            }

            var random = new Random();
            var selectedPets = unownedPets
                .OrderBy(p => random.Next())
                .Take(3)
                .Select(pet => new
                {
                    pet.PetId,
                    pet.Name,
                    pet.Type,
                    pet.Details,
                    pet.BattleStats,
                    pet.ImageUrl
                })
                .ToList();

            return Ok(selectedPets);
        }

        /// <summary>
        /// Devolve todos os pets marcados como favoritos na coleção do utilizador autenticado.
        /// </summary>
        /// <returns>Lista de pets favoritos.</returns>
        [HttpGet("favoritePets")]
        public async Task<ActionResult<IEnumerable<Pet>>> GetFavoritePetsInCollection()
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var collection = await _context.Collection
                .Include(c => c.CollectionPets)
                .ThenInclude(cp => cp.ReferencePet)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (collection == null)
            {
                collection = new Collection { UserId = userId, CollectionPets = new List<CollectionPets>()};
                _context.Collection.Add(collection);
                await _context.SaveChangesAsync();
            }

            var favoritePets = collection.CollectionPets
               .Where(cp => cp.IsFavorite)
               .Select(cp => cp.ReferencePet)
               .ToList();

            return Ok(favoritePets);
        }
        
    }

    /// <summary>
    /// Modelo utilizado para atualizar o estado de posse de um pet.
    /// </summary>
    public class UpdateOwnedStatusModel
    {
        public bool IsOwned { get; set; }
    }
}
