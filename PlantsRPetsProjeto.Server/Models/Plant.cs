namespace PlantsRPetsProjeto.Server.Models
{
    public class Plant
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? LastWatered { get; set; }
        public DateTime NextWatering { get; set; }
        public string GrowthStage { get; set; }
        public Pet Pet { get; set; }
    }

}
