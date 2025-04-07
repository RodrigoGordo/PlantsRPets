using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Data
{
    /// <summary>
    /// Seeder responsável por adicionar tutoriais educativos predefinidos à base de dados.
    /// Garante que os utilizadores têm acesso a conteúdos iniciais relacionados com jardinagem, compostagem e cuidados com plantas.
    /// </summary>
    public class TutorialSeeder
    {
        /// <summary>
        /// Insere tutoriais predefinidos na base de dados, caso ainda não existam.
        /// </summary>
        /// <param name="context">Contexto da base de dados da aplicação.</param>
        public static async Task SeedTutorials(PlantsRPetsProjetoServerContext context)
        {
            if (!await context.Tutorial.AnyAsync())
            {
                var tutorials = new List<Tutorial>
                {
                    new Tutorial
                    {
                        Title = "How to Start Your Own Garden",
                        Content = "Step 1: Choose a suitable location with adequate sunlight.\n" +
                                  "Step 2: Select the right plants for your environment.\n" +
                                  "Step 3: Prepare the soil by adding compost and other organic matter.\n" +
                                  "Step 4: Plant your seeds or seedlings at the right depth.\n" +
                                  "Step 5: Water your plants regularly but avoid over-watering.\n" +
                                  "Step 6: Maintain your garden by weeding, fertilizing, and pruning.",
                        AuthorId = "0"
                    },
                    new Tutorial
                    {
                        Title = "Indoor Plants Care",
                        Content = "Step 1: Choose plants that thrive in indoor environments, such as peace lilies or pothos.\n" +
                                  "Step 2: Place your plants in areas with adequate natural light.\n" +
                                  "Step 3: Water your plants only when the top inch of soil feels dry.\n" +
                                  "Step 4: Keep the humidity high by misting your plants or using a humidity tray.\n" +
                                  "Step 5: Prune dead leaves and stems to encourage new growth.\n" +
                                  "Step 6: Repot your plants when they outgrow their current pot.",
                        AuthorId = "0"
                    },
                    new Tutorial
                    {
                        Title = "Composting for Beginners",
                        Content = "Step 1: Choose a composting bin or make your own.\n" +
                                  "Step 2: Collect organic waste such as vegetable scraps, coffee grounds, and leaves.\n" +
                                  "Step 3: Layer your compost material with equal parts of brown (leaves) and green (food scraps) materials.\n" +
                                  "Step 4: Turn the compost every 1-2 weeks to promote aeration and speed up decomposition.\n" +
                                  "Step 5: Add water if the compost becomes too dry.\n" +
                                  "Step 6: Harvest your compost after 3-6 months when it turns dark and crumbly.",
                        AuthorId = "0"
                    },
                    new Tutorial
                    {
                        Title = "Organic Gardening Tips",
                        Content = "Step 1: Choose organic seeds or seedlings to avoid harmful chemicals.\n" +
                                  "Step 2: Use natural fertilizers such as compost or fish emulsion to feed your plants.\n" +
                                  "Step 3: Use organic pest control methods such as neem oil or ladybugs to keep pests away.\n" +
                                  "Step 4: Practice crop rotation to prevent soil depletion and pest buildup.\n" +
                                  "Step 5: Mulch around your plants to retain moisture and suppress weeds.\n" +
                                  "Step 6: Harvest plants at the right time to ensure the highest nutritional value.",
                        AuthorId = "0"
                    }
                };

                await context.Tutorial.AddRangeAsync(tutorials);
                await context.SaveChangesAsync();
            }
        }
    }
}
