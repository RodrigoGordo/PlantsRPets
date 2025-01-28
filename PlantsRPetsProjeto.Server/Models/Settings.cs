namespace PlantsRPetsProjeto.Server.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ICollection<string> NotificationPreferences { get; set; }
        public string Theme { get; set; }
        public string Language { get; set; }
        public string PrivacyOptions { get; set; }
    }
}
