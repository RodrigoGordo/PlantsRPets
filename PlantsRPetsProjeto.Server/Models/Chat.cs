namespace PlantsRPetsProjeto.Server.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public Community Community { get; set; }
        public List<Message> Messages { get; set; }
    }

}
