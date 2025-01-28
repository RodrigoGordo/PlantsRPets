namespace PlantsRPetsProjeto.Server.Models
{
    public class Metric
    {
        public int MetricId { get; set; }
        public int UserId { get; set; }
        public int TotalPlants { get; set; }
        public decimal WaterSaved { get; set; }
        public decimal CarbonFootprintReduction { get; set; }
    }
}
