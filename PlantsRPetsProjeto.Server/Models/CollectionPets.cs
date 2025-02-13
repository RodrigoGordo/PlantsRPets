using System.Threading.Tasks.Dataflow;

namespace PlantsRPetsProjeto.Server.Models
{
    public class CollectionPets
    {
        public int CollectionPetsId { get; set; }
        public int CollectionId { get; set; }
        public virtual Collection ReferenceCollection { get; set; }
        public int PetId { get; set; }
        public virtual Pet ReferencePet { get; set; }
        public bool IsFavorite { get; set; }
    }
}
