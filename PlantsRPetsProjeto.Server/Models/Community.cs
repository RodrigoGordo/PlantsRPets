namespace PlantsRPetsProjeto.Server.Models
{
    public class Community
    {
        public int CommunityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public ICollection<User> Members { get; set; }
    }

}
