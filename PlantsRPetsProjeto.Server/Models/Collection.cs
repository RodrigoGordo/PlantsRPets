namespace PlantsRPetsProjeto.Server.Models
{
    public class Collection
    {
        public int CollectionId { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CollectionPets> CollectionPets { get; set; }
    }

}
