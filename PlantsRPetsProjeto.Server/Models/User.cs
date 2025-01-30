using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PlantsRPetsProjeto.Server.Models
{
    public class User : IdentityUser
    {
        [PersonalData][Required][Display(Name = "Nickname")]
        public string Nickname { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Profile? Profile { get; set; }
        public ICollection<Plantation> Plantations { get; set; }
        public ICollection<Pet> Pets { get; set; }
        public Dashboard? Dashboard { get; set; }
        public ICollection<Community> Communities { get; set; }

    }
}