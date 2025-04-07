namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa um tipo de planta (ex: árvore, flor, erva).
    /// Inclui informação sobre se o tipo permite colheitas recorrentes.
    /// </summary>
    public class PlantType
    {
        /// <summary>
        /// Identificador único do tipo de planta.
        /// </summary>
        public int PlantTypeId { get; set; }

        /// <summary>
        /// Nome do tipo de planta (ex: "árvore", "vegetal").
        /// </summary>
        public string? PlantTypeName { get; set; }

        /// <summary>
        /// Indica se este tipo de planta permite colheitas recorrentes ao longo do tempo.
        /// </summary>
        public bool HasRecurringHarvest { get; set; }
    }
}
