namespace PlantsRPetsProjeto.Server.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Profile Profile { get; set; }
        public ICollection<Plantation> Plantations { get; set; }
        public ICollection<Pet> Pets { get; set; }
        public Dashboard Dashboard { get; set; }
        public ICollection<Community> Communities { get; set; }

    }
}