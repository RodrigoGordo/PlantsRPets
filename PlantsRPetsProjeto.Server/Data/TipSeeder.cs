using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Data
{
    public class TipSeeder
    {
        public static async Task SeedSustainabilityTips(PlantsRPetsProjetoServerContext context)
        {
            if (!await context.SustainabilityTip.AnyAsync())
            {
                var tips = new List<SustainabilityTip>
                {
                    new SustainabilityTip
                    {
                        Title = "Compost Your Waste",
                        Content = "Turn organic waste into compost to reduce landfill waste and enrich your soil.",
                        Category = "Waste Management",
                        AuthorId = "0"
                    },
                    new SustainabilityTip
                    {
                        Title = "Use Rainwater",
                        Content = "Collect rainwater for watering plants to conserve tap water.",
                        Category = "Water Conservation",
                        AuthorId = "0"
                    },
                    new SustainabilityTip
                    {
                        Title = "Grow Your Own Food",
                        Content = "Plant vegetables and herbs to reduce carbon footprint and promote sustainability.",
                        Category = "Self-Sustainability",
                        AuthorId = "0"
                    },
                    new SustainabilityTip
                    {
                        Title = "Use Organic Pest Control",
                        Content = "Encourage beneficial insects like ladybugs and plant marigolds to naturally repel harmful pests.",
                        Category = "Pest Management",
                        AuthorId = "0"
                    }
                };

                await context.SustainabilityTip.AddRangeAsync(tips);
                await context.SaveChangesAsync();
            }
        }
    }
}
