using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantsRPetsProjeto.Server.Models
{
    public class PlantInfo
    {
        public int PlantInfoId { get; set; }
        public string PlantName { get; set; }
        public string PlantType { get; set; }
        public string Cycle { get; set; }
        public string Watering { get; set; }
        public List<string> PruningMonth { get; set; }
        public PruningCountInfo? PruningCount { get; set; }
        public string GrowthRate { get; set; }
        public List<string> Sunlight { get; set; }
        public string Edible { get; set; }
        public string CareLevel { get; set; }
        public string Flowers { get; set; }
        public string Fruits { get; set; }
        public bool Leaf { get; set; }
        public string Maintenance { get; set; }
        public string SaltTolerant { get; set; }
        public bool Indoor { get; set; }
        public string FloweringSeason { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public string HarvestSeason { get; set; }
        public List<string> ScientificName { get; set; }
        public bool DroughtTolerant { get; set; }
        public bool Cuisine { get; set; }
        public bool Medicinal { get; set; }

    }

}