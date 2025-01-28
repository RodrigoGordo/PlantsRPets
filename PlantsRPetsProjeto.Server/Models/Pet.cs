namespace PlantsRPetsProjeto.Server.Models
{
    public class Pet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public bool IsFavorite { get; set; }
    }
}
