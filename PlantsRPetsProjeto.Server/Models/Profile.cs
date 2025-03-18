namespace PlantsRPetsProjeto.Server.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public string UserId { get; set; }
        public string? Bio { get; set; }

        public string? ProfilePicture { get; set; }

        public ICollection<int>? FavoritePets { get; set; }

        public ICollection<int>? HighlightedPlantations { get; set; }
    }
}
