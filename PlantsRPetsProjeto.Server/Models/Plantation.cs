using System.ComponentModel.DataAnnotations.Schema;

namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa uma plantação pertencente a um utilizador.
    /// Agrupa várias plantas cultivadas sob o mesmo contexto e tipo.
    /// </summary>
    public class Plantation
    {
        /// <summary>
        /// Identificador único da plantação.
        /// </summary>
        public int PlantationId { get; set; }

        /// <summary>
        /// Identificador do utilizador proprietário da plantação.
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// Utilizador a que pertence a plantação.
        /// </summary>
        [ForeignKey("OwnerId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Nome personalizado atribuído pelo utilizador à plantação.
        /// </summary>
        public string PlantationName { get; set; }

        /// <summary>
        /// Identificador do tipo de planta predominante nesta plantação.
        /// </summary>
        public int PlantTypeId { get; set; }

        /// <summary>
        /// Referência ao tipo de planta cultivado.
        /// </summary>
        public virtual PlantType PlantType { get; set; }

        /// <summary>
        /// Data de início da plantação.
        /// </summary>
        public DateTime PlantingDate { get; set; }

        /// <summary>
        /// Pontos de experiência acumulados pela plantação através de ações como rega e colheita.
        /// </summary>
        public int ExperiencePoints { get; set; }

        /// <summary>
        /// Nível atual da plantação, com base na experiência obtida.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Níveis ganhos mas ainda não aplicados à plantação.
        /// </summary>
        public int BankedLevelUps { get; set; }

        /// <summary>
        /// Localização geográfica onde está inserida a plantação
        /// </summary>
        public Location? Location { get; set; }

        /// <summary>
        /// Coleção de plantas cultivadas nesta plantação.
        /// </summary>
        public virtual ICollection<PlantationPlants> PlantationPlants { get; set; }
    }
}
