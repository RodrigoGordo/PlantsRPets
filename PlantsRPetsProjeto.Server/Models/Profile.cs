namespace PlantsRPetsProjeto.Server.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public string UserId { get; set; }
        public string? Bio { get; set; }

        public string? ProfilePicture { get; set; }

        public ICollection<Pet>? FavoritePets { get; set; }

        public ICollection<Plantation>? HighlightedPlantations { get; set; }
    }
}
