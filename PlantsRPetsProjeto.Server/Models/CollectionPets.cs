using System.Threading.Tasks.Dataflow;

namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa a associação entre um animal virtual (Pet) e uma coleção de um utilizador.
    /// Contém informações adicionais sobre o estado do animal dentro da coleção.
    /// </summary>
    public class CollectionPets
    {
        /// <summary>
        /// Identificador único da associação entre o pet e a coleção.
        /// </summary>
        public int CollectionPetsId { get; set; }

        /// <summary>
        /// Identificador da coleção à qual o pet pertence.
        /// </summary>
        public int CollectionId { get; set; }

        /// <summary>
        /// Referência à coleção a que este pet pertence.
        /// </summary>
        public virtual Collection ReferenceCollection { get; set; }

        /// <summary>
        /// Identificador do pet associado à coleção.
        /// </summary>
        public int PetId { get; set; }

        /// <summary>
        /// Referência ao pet associado.
        /// </summary>
        public virtual Pet ReferencePet { get; set; }

        /// <summary>
        /// Indica se o pet está marcado como favorito na coleção.
        /// </summary>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// Indica se o utilizador já obteve (desbloqueou) este pet.
        /// </summary>
        public bool IsOwned { get; set; }
    }
}
