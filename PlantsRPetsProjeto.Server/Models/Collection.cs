namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa a coleção de animais virtuais (pets) pertencente a um utilizador.
    /// Permite marcar favoritos e controlar quais pets foram adquiridos.
    /// </summary>
    public class Collection
    {
        /// <summary>
        /// Identificador único da coleção.
        /// </summary>
        public int CollectionId { get; set; }

        /// <summary>
        /// Identificador do utilizador proprietário da coleção.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Referência ao utilizador proprietário da coleção.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Lista de pets incluídos na coleção do utilizador.
        /// </summary>
        public virtual ICollection<CollectionPets> CollectionPets { get; set; }
    }

}
