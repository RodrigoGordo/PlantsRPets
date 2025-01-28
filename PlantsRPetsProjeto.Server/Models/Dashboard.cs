namespace PlantsRPetsProjeto.Server.Models
{
    public class Dashboard
    {
        public int DashboardId { get; set; }
        public int UserId { get; set; }
        public ICollection<Metric> DashboardMetrics { get; set; }

    }
 
}
