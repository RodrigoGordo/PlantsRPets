using Microsoft.AspNetCore.Mvc;

namespace PlantsRPetsProjeto.Server.Models
{
    public class Plant
    {
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public string PlantType { get; set; }
        public string Cycle { get; set; }
        public string Watering { get; set; }
        public List<string> PruningMonth { get; set; }
        public string PruningCount { get; set; }
        public string GrowthRate { get; set; }
        public string Sun { get; set; }
        public string Edible { get; set; }
        public string CareLevel { get; set; }
        public string Flowers { get; set; }
        public string Fruits { get; set; }
        public bool Leaf { get; set; }
        public string Maintenance { get; set; }
        public string SaltTolerant { get; set; }
        public bool Indoor { get; set; }
        public string SunDuration { get; set; }
        public string FloweringSeason { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

}
