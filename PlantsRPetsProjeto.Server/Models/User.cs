using Microsoft.AspNetCore.Identity;

namespace PlantsRPetsProjeto.Server.Models
{
    public class User : IdentityUser
    {

        public DateTime RegistrationDate { get; set; }
        public Profile Profile { get; set; }
        public ICollection<Plantation> Plantations { get; set; }
        public ICollection<Pet> Pets { get; set; }
        public Dashboard Dashboard { get; set; }
        public ICollection<Community> Communities { get; set; }

    }
}