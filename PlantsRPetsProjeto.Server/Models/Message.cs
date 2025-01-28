namespace PlantsRPetsProjeto.Server.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public int ChatId { get; set; }

    }

}
