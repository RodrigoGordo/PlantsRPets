namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa a associação entre uma plantação e as plantas que nela foram cultivadas.
    /// Contém informação detalhada sobre cada instância de plantação de uma planta específica.
    /// </summary>
    public class PlantationPlants
    {
        /// <summary>
        /// Identificador único da entrada que representa a planta na plantação.
        /// </summary>
        public int PlantationPlantsId { get; set; }

        /// <summary>
        /// Identificador da plantação onde a planta está inserida.
        /// </summary>
        public int PlantationId { get; set; }

        /// <summary>
        /// Referência à plantação correspondente.
        /// </summary>
        public virtual Plantation ReferencePlantation { get; set; }

        /// <summary>
        /// Identificador da planta cultivada.
        /// </summary>
        public int PlantInfoId { get; set; }

        /// <summary>
        /// Referência à informação da planta cultivada.
        /// </summary>
        public virtual PlantInfo ReferencePlant { get; set; }

        /// <summary>
        /// Quantidade da planta cultivada nesta plantação.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Data em que a planta foi cultivada.
        /// </summary>
        public DateTime PlantingDate { get; set; }

        /// <summary>
        /// Data da última rega da planta (pode ser nula caso ainda não tenha sido regada).
        /// </summary>
        public DateTime? LastWatered { get; set; }

        /// <summary>
        /// Data da última colheita realizada (caso aplicável).
        /// </summary>
        public DateTime? LastHarvested { get; set; }

        /// <summary>
        /// Próxima data estimada de colheita da planta.
        /// </summary>
        public DateTime? HarvestDate { get; set; }

        /// <summary>
        /// Estado atual de crescimento da planta (ex: "Germinação", "Crescimento", "Pronta para colheita").
        /// </summary>
        public string GrowthStatus { get; set; }
    }
}
