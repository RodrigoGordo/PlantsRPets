using System.ComponentModel.DataAnnotations.Schema;

namespace PlantsRPetsProjeto.Server.Models
{
    public class Plantation
    {
        public int PlantationId { get; set; }
        public string OwnerId { get; set; }
        public string PlantationName { get; set; }
        public string PlantType { get; set; }
        public DateTime PlantingDate { get; set; }
        public DateTime LastWatered { get; set; }
        public DateTime HarvestDate { get; set; }
        public string GrowthStatus { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public virtual ICollection<PlantationPlants> PlantationPlants { get; set; }
    }
}
