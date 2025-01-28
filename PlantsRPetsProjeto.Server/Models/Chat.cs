namespace PlantsRPetsProjeto.Server.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public int CommunityId { get; set; }
        public ICollection<Message> Messages { get; set; }
    }

}
