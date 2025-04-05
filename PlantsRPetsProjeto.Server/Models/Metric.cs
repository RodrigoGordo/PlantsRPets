namespace PlantsRPetsProjeto.Server.Models
{
    public class Metric
    {
        public int MetricId { get; set; }
        public string UserId { get; set; }
        public int PlantationId { get; set; }
        public int? PlantInfoId { get; set; }
        public string EventType { get; set; }
        public DateTime Timestamp { get; set; }
    }
}