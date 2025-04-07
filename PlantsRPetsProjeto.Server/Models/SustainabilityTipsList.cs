using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Lista de dicas de sustentabilidade associadas a uma planta.
    /// Agrupa sugestões relevantes por planta.
    /// </summary>
    public class SustainabilityTipsList
    {
        /// <summary>
        /// Identificador único da lista de dicas.
        /// </summary>
        public int SustainabilityTipsListId { get; set; }

        /// <summary>
        /// Identificador da planta à qual as dicas estão associadas.
        /// </summary>
        public int PlantInfoId { get; set; }

        /// <summary>
        /// Nome comum da planta.
        /// </summary>
        public string PlantName { get; set; }

        /// <summary>
        /// Lista de nomes científicos da planta.
        /// </summary>
        public IList<string> PlantScientificName { get; set; }

        /// <summary>
        /// Lista de dicas de sustentabilidade associadas à planta.
        /// </summary>
        public IList<SustainabilityTip> SustainabilityTip { get; set; }
    }
}
