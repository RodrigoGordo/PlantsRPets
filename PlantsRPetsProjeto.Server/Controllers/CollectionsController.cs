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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollectionsController : Controller
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public CollectionsController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetUserCollection()
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Get or create user's collection
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

        [HttpPost("add/{petId}")]
        public async Task<IActionResult> AddPetToCollection(int petId)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Check if pet exists
            var pet = await _context.Pet.FindAsync(petId);
            if (pet == null)
            {
                return NotFound("Pet not found");
            }

            // Get or create user's collection
            var collection = await _context.Collection
                .Include(c => c.CollectionPets)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (collection == null)
            {
                collection = new Collection { UserId = userId };
                _context.Collection.Add(collection);
                await _context.SaveChangesAsync();
            }

            // Check if pet is already in collection
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

            collectionPet.IsFavorite = !collectionPet.IsFavorite;
            await _context.SaveChangesAsync();

            return Ok(new { isFavorite = collectionPet.IsFavorite });
        }
    }
}
