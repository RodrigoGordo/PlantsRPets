namespace PlantsRPetsProjeto.Server.Models
{
    public class PlantationPlants
    {
        public int PlantationPlantsId { get; set; }
        public int PlantationId { get; set; }
        public virtual Plantation ReferencePlantation { get; set; }
        public int PlantId { get; set; }
        public virtual Plant ReferencePlant { get; set; }
        public int Quantity { get; set; }
    }
}
