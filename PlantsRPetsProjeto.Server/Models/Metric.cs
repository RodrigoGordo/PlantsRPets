namespace PlantsRPetsProjeto.Server.Models
{
    public class Metric
    {
        public int MetricId { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int WateringCount { get; set; }
        public int HarvestCount { get; set; }
        public int PlantingCount { get; set; }
        public int PlantationId { get; set; }
        public int? PlantTypeId { get; set; }
        public virtual PlantType? PlantType { get; set; }

    }
}
