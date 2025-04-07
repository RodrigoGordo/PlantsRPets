using System.ComponentModel.DataAnnotations;

namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa a frequência e quantidade de podas recomendadas para uma determinada planta.
    /// </summary>
    public class PruningCountInfo
    {
        /// <summary>
        /// Identificador único da informação de poda.
        /// </summary>
        public int PruningCountInfoId { get; set; }

        /// <summary>
        /// Número de vezes que a planta deve ser podada num determinado intervalo de tempo.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Intervalo temporal entre podas (ex: "mensal", "semanal").
        /// </summary>
        public string Interval { get; set; }
    }
}
