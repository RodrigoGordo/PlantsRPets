using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa uma dica de sustentabilidade individual.
    /// Pode ser sobre rega, poda, compostagem, entre outros.
    /// </summary>
    public class SustainabilityTip
    {
        /// <summary>
        /// Identificador único da dica.
        /// </summary>
        public int SustainabilityTipId { get; set; }

        /// <summary>
        /// Tipo ou categoria da dica (ex: "Rega", "Solo", "Iluminação").
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Texto descritivo da dica de sustentabilidade.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Chave estrangeira que liga esta dica à sua lista associada.
        /// </summary>
        public int SustainabilityTipsListId { get; set; }
    }

}
