namespace PlantsRPetsProjeto.Server.Models
{
    public class PlantationPlants
    {
        public int PlantationPlantsId { get; set; }
        public int PlantationId { get; set; }
        public virtual Plantation ReferencePlantation { get; set; }
        public int PlantInfoId { get; set; }
        public virtual PlantInfo ReferencePlant { get; set; }
        public int Quantity { get; set; }
        public DateTime PlantingDate { get; set; }
        public DateTime? LastWatered { get; set; }
        public DateTime? HarvestDate { get; set; }
        public string GrowthStatus { get; set; }
    }
}
