using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;


namespace PlantsRPetsProjeto.Server.Data
{
    public class PetSeeder
    {
        private readonly PlantsRPetsProjetoServerContext _context;
        private readonly PetGeneratorService _petGeneratorService;
        private readonly UserManager<User> _userManager;

        public PetSeeder(PlantsRPetsProjetoServerContext context, PetGeneratorService petGeneratorService, UserManager<User> userManager)
        {
            _context = context;
            _petGeneratorService = petGeneratorService;
            _userManager = userManager;
        }

        public async Task SeedAsync(int numberOfPets = 40)
        {
            if (!await _context.Pet.AnyAsync())
            {
                var allPets = new List<Pet>();
                for (int i = 0; i < numberOfPets; i++)
                {
                    var pet = await _petGeneratorService.GeneratePetAsync();

                    if (pet != null)
                    {
                        await Task.Delay(500);
                        if (allPets.Contains(pet))
                        {
                            i--;
                            continue;
                        }
                        _context.Pet.Add(pet);
                        allPets.Add(pet);
                    } else
                    {
                        i--;
                    }
                }

                await _context.SaveChangesAsync();
            }

            await SetupAdminCollectionsAsync();
        }


        private async Task SetupAdminCollectionsAsync()
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");

            if (!adminUsers.Any())
            {
                return;
            }

            // Get all available pets
            var allPets = await _context.Pet.ToListAsync();

            foreach (var admin in adminUsers)
            {
                var adminCollection = await _context.Collection
                    .Include(c => c.CollectionPets)
                    .FirstOrDefaultAsync(c => c.UserId == admin.Id);

                if (adminCollection == null)
                {
                    // Create new collection for admin
                    adminCollection = new Collection
                    {
                        UserId = admin.Id,
                        CollectionPets = new List<CollectionPets>()
                    };
                    _context.Collection.Add(adminCollection);
                }

                // Add all pets to admin's collection
                foreach (var pet in allPets)
                {
                    if (!adminCollection.CollectionPets.Any(cp => cp.PetId == pet.PetId))
                    {
                        adminCollection.CollectionPets.Add(new CollectionPets
                        {
                            PetId = pet.PetId,
                            IsOwned = true,
                            IsFavorite = false
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}

