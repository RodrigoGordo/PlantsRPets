using System.Threading.Tasks;
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

        public PetSeeder(PlantsRPetsProjetoServerContext context, PetGeneratorService petGeneratorService)
        {
            _context = context;
            _petGeneratorService = petGeneratorService;
        }

        public async Task SeedAsync(int numberOfPets = 40)
        {
            if (!await _context.Pet.AnyAsync())
            {
                for (int i = 0; i < numberOfPets; i++)
                {
                    var pet = await _petGeneratorService.GeneratePetAsync();
                    _context.Pet.Add(pet);
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
