namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa a associação entre um utilizador e uma notificação recebida.
    /// Armazena o estado de leitura e a data de receção.
    /// </summary>
    public class UserNotification
    {
        /// <summary>
        /// Identificador único da notificação do utilizador.
        /// </summary>
        public int UserNotificationId { get; set; }

        /// <summary>
        /// Referência ao utilizador que recebeu a notificação.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Chave estrangeira do utilizador associado.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Referência à notificação recebida.
        /// </summary>
        public virtual Notification Notification { get; set; }

        /// <summary>
        /// Chave estrangeira da notificação associada.
        /// </summary>
        public int NotificationId { get; set; }

        /// <summary>
        /// Data e hora em que a notificação foi recebida.
        /// </summary>
        public DateTime ReceivedDate { get; set; }

        /// <summary>
        /// Indica se a notificação foi lida pelo utilizador.
        /// </summary>
        public bool isRead { get; set; }
    }
}
