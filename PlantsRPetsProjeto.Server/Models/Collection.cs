namespace PlantsRPetsProjeto.Server.Models
{
    public class Collection
    {
        public int CollectionId { get; set; }
        public ICollection<Pet> CollectedPets { get; set; }
    }

}
