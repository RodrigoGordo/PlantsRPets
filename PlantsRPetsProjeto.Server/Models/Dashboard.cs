namespace PlantsRPetsProjeto.Server.Models
{
    public class Dashboard
    {
        public int DashboardId { get; set; }
        public string UserId { get; set; }
        public ICollection<Metric> DashboardMetrics { get; set; }

    }
 
}
