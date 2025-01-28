namespace PlantsRPetsProjeto.Server.Models
{
    public class DashboardMetrics
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int TotalPlants { get; set; }
        public decimal WaterSaved { get; set; }
        public decimal CarbonFootprintReduction { get; set; }
    }
}
