namespace PlantsRPetsProjeto.Server.Models
{
    public class PlantType
    {
        public int PlantTypeId { get; set; }
        public string? PlantTypeName { get; set; }
        public bool HasRecurringHarvest { get; set; }
    }
}
