namespace PlantsRPetsProjeto.Server.Models
{
    public class Plantation
    {
        public int Id { get; set; }
        public Plant Plant { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public DateTime PlantingDate { get; set; }
        public DateTime LastWatered { get; set; }
        public string GrowthStatus { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
    }
}
