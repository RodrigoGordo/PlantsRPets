using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;


namespace PlantsRPetsProjeto.Server.Data
{
    /// <summary>
    /// Seeder responsável por gerar e inserir pets aleatórios na base de dados.
    /// Também assegura que os utilizadores com o papel de administrador possuem todos os pets disponíveis na sua coleção.
    /// </summary>
    public class PetSeeder
    {
        private readonly PlantsRPetsProjetoServerContext _context;
        private readonly PetGeneratorService _petGeneratorService;
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Construtor do <see cref="PetSeeder"/>.
        /// </summary>
        /// <param name="context">Contexto da base de dados.</param>
        /// <param name="petGeneratorService">Serviço responsável por gerar pets aleatórios.</param>
        /// <param name="userManager">Gestor de utilizadores da aplicação.</param>
        public PetSeeder(PlantsRPetsProjetoServerContext context, PetGeneratorService petGeneratorService, UserManager<User> userManager)
        {
            _context = context;
            _petGeneratorService = petGeneratorService;
            _userManager = userManager;
        }

        /// <summary>
        /// Executa o processo de seed dos pets e associa-os aos administradores.
        /// </summary>
        /// <param name="numberOfPets">Número de pets a gerar (valor por omissão: 40).</param>
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


        /// <summary>
        /// Associa todos os pets existentes à coleção dos administradores.
        /// Se a coleção ainda não existir, é criada.
        /// </summary>
        private async Task SetupAdminCollectionsAsync()
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");

            if (!adminUsers.Any())
            {
                return;
            }

            var allPets = await _context.Pet.ToListAsync();

            foreach (var admin in adminUsers)
            {
                var adminCollection = await _context.Collection
                    .Include(c => c.CollectionPets)
                    .FirstOrDefaultAsync(c => c.UserId == admin.Id);

                if (adminCollection == null)
                {
                    adminCollection = new Collection
                    {
                        UserId = admin.Id,
                        CollectionPets = new List<CollectionPets>()
                    };
                    _context.Collection.Add(adminCollection);
                }

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

