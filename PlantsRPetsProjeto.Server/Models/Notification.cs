namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa uma notificação genérica enviada ao utilizador.
    /// Pode conter lembretes, alertas ou recompensas.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Identificador único da notificação.
        /// </summary>
        public int NotificationId { get; set; }

        /// <summary>
        /// Tipo da notificação (ex: "Lembrete", "Alerta de Colheita", "Recompensa").
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Conteúdo da mensagem enviada ao utilizador.
        /// </summary>
        public string Message { get; set; }
    }
}
