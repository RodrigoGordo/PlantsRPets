namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Regista eventos relevantes realizados por um utilizador,
    /// como ações em plantações (rega, colheita, plantação, etc.).
    /// </summary>
    public class Metric
    {
        /// <summary>
        /// Identificador único da métrica.
        /// </summary>
        public int MetricId { get; set; }

        /// <summary>
        /// Identificador do utilizador associado ao evento.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Identificador da plantação onde ocorreu o evento.
        /// </summary>
        public int PlantationId { get; set; }

        /// <summary>
        /// Identificador da planta associada ao evento (pode ser nulo em alguns tipos de evento).
        /// </summary>
        public int? PlantInfoId { get; set; }

        /// <summary>
        /// Tipo de evento registado (ex: "Watering", "Harvest", "Planting").
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Momento em que o evento ocorreu.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Quantidade de vezes que foi chamada a métrica
        /// </summary>
        public int? Quantity { get; set; } = 1;
    }
}