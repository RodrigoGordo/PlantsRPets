namespace PlantsRPetsProjeto.Server.Models
{
    public class Collection
    {
        public int CollectionId { get; set; }
        public virtual ICollection<CollectionPets> CollectionPets { get; set; }
    }

}
