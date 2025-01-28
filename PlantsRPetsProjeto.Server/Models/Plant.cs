namespace PlantsRPetsProjeto.Server.Models
{
    public class Plant
    {
        public int PlantId { get; set; }
        public int SustainabilityInfoId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int GrowthTime { get; set; }
        public int WaterFrequency { get; set; }
        public bool isGrown { get; set; }
    }

}
