namespace PlantsRPetsProjeto.Server.Models
{
    public class Metric
    {
        public int MetricId { get; set; }
        public int UserId { get; set; }
        public int TotalPlants { get; set; }
        public double WaterSaved { get; set; }
        public double CarbonFootprintReduction { get; set; }
    }
}
