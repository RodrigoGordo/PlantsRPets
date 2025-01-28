namespace PlantsRPetsProjeto.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PassswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Profile Profile { get; set; }
        public List<Plantation> Plantations { get; set; }
        public List<Pet> Pets { get; set; }
        public Dashboard Dashboard { get; set; }

    }
}