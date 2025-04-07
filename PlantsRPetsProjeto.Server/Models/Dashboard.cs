namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa o painel estatístico de um utilizador,
    /// agregando métricas de atividade na aplicação.
    /// </summary>
    public class Dashboard
    {
        /// <summary>
        /// Identificador único do painel de controlo.
        /// </summary>
        public int DashboardId { get; set; }

        /// <summary>
        /// Identificador do utilizador ao qual este dashboard pertence.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Coleção de métricas associadas ao utilizador para visualização estatística.
        /// </summary>
        public ICollection<Metric> DashboardMetrics { get; set; }
    }

}
