using System.Threading.Tasks.Dataflow;

namespace PlantsRPetsProjeto.Server.Models
{
    public class CollectionPets
    {
        public int CollectionPetsId { get; set; }
        public int CollectionId { get; set; }
        public virtual Collection Collection { get; set; }
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }
        public bool IsFavorite { get; set; }
    }
}
