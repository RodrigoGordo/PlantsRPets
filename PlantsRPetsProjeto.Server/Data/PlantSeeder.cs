using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Data
{
    public class PlantSeeder
    {
        public static async Task SeedPlants(PlantsRPetsProjetoServerContext context)
        {
            if (!await context.PlantInfo.AnyAsync())
            {
                var plantInfo = new List<PlantInfo>
                {
                    new PlantInfo
                    {
                        PlantName = "Kiwifruit",
                        PlantType = "Fruit",
                        Cycle = "Perennial",
                        Watering = "0 days",
                        PruningMonth = ["February", "June", "July"],
                        PruningCount = "2 times",
                        GrowthRate = "High",
                        Sun = "Full sun",
                        Edible = "True",
                        CareLevel = "Easy",
                        Flowers = "True",
                        Fruits = "True",
                        Leaf = true,
                        Maintenance = "Moderate",
                        SaltTolerant = "False",
                        Indoor = false,
                        SunDuration = "4 hours",
                        FloweringSeason = "Spring",
                        Description = "Kiwifruit (Actinidia deliciosa) is an amazing fruit packed with nutrients. " +
                        "It contains an impressive amount of vitamin C, more than an equal amount of oranges. " +
                        "Kiwifruit is also loaded with fiber and micronutrients, like vitamin E, magnesium, and potassium. " +
                        "Its unique flavor is sweet and tangy, making it an ideal addition to many dishes. " +
                        "The fuzzy green skin can be eaten and provides added fiber, vitamins and minerals. " +
                        "Enjoy kiwifruit as part of a healthy and diverse diet for an extra boosted of nutrition and flavor.",
                        Image = "https://perenual.com/storage/species_image/536_actinidia_deliciosa/og/25119432356_f10218d971_b.jpg",
                    },

                    new PlantInfo
                    {
                        PlantName = "Barberry",
                        PlantType = "Deciduous shrub",
                        Cycle = "Perennial",
                        Watering = "Minimum",
                        PruningMonth = ["February", "March", "April"],
                        PruningCount = "1 time yearly",
                        GrowthRate = "Low",
                        Sun = "Full sun, Part shade",
                        Edible = "True",
                        CareLevel = "Unknown",
                        Flowers = "True",
                        Fruits = "True",
                        Leaf = true,
                        Maintenance = "Low",
                        SaltTolerant = "True",
                        Indoor = false,
                        SunDuration = "Unknown",
                        FloweringSeason = "Spring",
                        Description = "Barberry (Berberis mentorensis) is an amazing evergreen plant with beautiful, bright green foliage that brightens up any garden. " +
                        "It produces colorful flowers followed by red berries that attract birds. The plant prefers full sun and well-drained soil, making it versatile and easy to grow. " +
                        "The foliage and twigs are also evergreen, keeping the garden bright year-round. Barberry is also known for its medicinal properties, making it not only a stunning garden plant but a useful one as well. " +
                        "Overall, barberry is a perfect choice for any garden, providing year-round beauty, color, and even helpful medicinal benefits.",
                        Image = "https://perenual.com/storage/species_image/1236_berberis_mentorensis/og/52376978258_9f1dea2d55_b.jpg",
                    },

                };

                await context.PlantInfo.AddRangeAsync(plantInfo);
                await context.SaveChangesAsync();
            }
        }
    }
}
