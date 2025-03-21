using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PlantsRPetsProjeto.Server.Controllers;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using Xunit;

namespace PlantsRPetsProjeto.Tests.xUnitTests.ControllerTests
{
    public class PetsControllerTests
    {
        [Fact]
        public async Task GetPets_ReturnsAllPets()
        {
            var testPets = GetTestPets();
            var dbContextOptions = new DbContextOptionsBuilder<PlantsRPetsProjetoServerContext>()
                .UseInMemoryDatabase(databaseName: "TestPetsDb_GetAll")
                .Options;

            using (var context = new PlantsRPetsProjetoServerContext(dbContextOptions))
            {
                context.Pet.AddRange(testPets);
                context.SaveChanges();
            }

            using (var context = new PlantsRPetsProjetoServerContext(dbContextOptions))
            {
                var controller = new PetsController(context);

                var result = await controller.GetPets();

                var actionResult = Assert.IsType<ActionResult<IEnumerable<Pet>>>(result);
                var pets = Assert.IsAssignableFrom<IEnumerable<Pet>>(actionResult.Value);
                Assert.Equal(testPets.Count, pets.Count());
            }
        }

        [Fact]
        public async Task GetPet_WithValidId_ReturnsPet()
        {
            var testPets = GetTestPets();
            var targetPet = testPets.First();
            var dbContextOptions = new DbContextOptionsBuilder<PlantsRPetsProjetoServerContext>()
                .UseInMemoryDatabase(databaseName: "TestPetsDb_GetById")
                .Options;

            using (var context = new PlantsRPetsProjetoServerContext(dbContextOptions))
            {
                context.Pet.AddRange(testPets);
                context.SaveChanges();
            }

            using (var context = new PlantsRPetsProjetoServerContext(dbContextOptions))
            {
                var controller = new PetsController(context);

                var result = await controller.GetPet(targetPet.PetId);

                var actionResult = Assert.IsType<ActionResult<Pet>>(result);
                var pet = Assert.IsType<Pet>(actionResult.Value);
                Assert.Equal(targetPet.PetId, pet.PetId);
                Assert.Equal(targetPet.Name, pet.Name);
                Assert.Equal(targetPet.ImageUrl, pet.ImageUrl);
            }
        }

        [Fact]
        public async Task GetPet_WithInvalidId_ReturnsNotFound()
        {
            var invalidId = 999;
            var dbContextOptions = new DbContextOptionsBuilder<PlantsRPetsProjetoServerContext>()
                .UseInMemoryDatabase(databaseName: "TestPetsDb_GetByInvalidId")
                .Options;

            using (var context = new PlantsRPetsProjetoServerContext(dbContextOptions))
            {
                var controller = new PetsController(context);

                var result = await controller.GetPet(invalidId);

                var actionResult = Assert.IsType<ActionResult<Pet>>(result);
                Assert.IsType<NotFoundResult>(actionResult.Result);
            }
        }

        private List<Pet> GetTestPets()
        {
            return new List<Pet>
            {
                new Pet
                {
                    PetId = 1,
                    Name = "🍉😀 Pet",
                    Type = "Plant/Fruit/Vegetable + Animal/Face",
                    Details = "A unique pet made from 🍉 and 😀.",
                    BattleStats = "Health: 80, Attack: 20, Defense: 15, Speed: 25",
                    ImageUrl = "https://emojik.vercel.app/s/🍉_😀?size=256"
                },
                new Pet
                {
                    PetId = 2,
                    Name = "🌻🐶 Pet",
                    Type = "Plant/Fruit/Vegetable + Animal/Face",
                    Details = "A unique pet made from 🌻 and 🐶.",
                    BattleStats = "Health: 70, Attack: 15, Defense: 25, Speed: 20",
                    ImageUrl = "https://emojik.vercel.app/s/🌻_🐶?size=256"
                },
                new Pet
                {
                    PetId = 3,
                    Name = "🍓🐱 Pet",
                    Type = "Plant/Fruit/Vegetable + Animal/Face",
                    Details = "A unique pet made from 🍓 and 🐱.",
                    BattleStats = "Health: 65, Attack: 25, Defense: 15, Speed: 30",
                    ImageUrl = "https://emojik.vercel.app/s/🍓_🐱?size=256"
                }
            };
        }
    }
}