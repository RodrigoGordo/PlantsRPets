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
                        AuthorId = "system"
                    },
                    new SustainabilityTip
                    {
                        Title = "Use Rainwater",
                        Content = "Collect rainwater for watering plants to conserve tap water.",
                        Category = "Water Conservation",
                        AuthorId = "system"
                    },
                    new SustainabilityTip
                    {
                        Title = "Grow Your Own Food",
                        Content = "Plant vegetables and herbs to reduce carbon footprint and promote sustainability.",
                        Category = "Self-Sustainability",
                        AuthorId = "system"
                    }
                };

                await context.SustainabilityTip.AddRangeAsync(tips);
                await context.SaveChangesAsync();
            }
        }
    }
}
