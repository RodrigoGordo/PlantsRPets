namespace PlantsRPetsProjeto.Server.Models
{
    public class UserNotifications
    {
        public int UserNotificationsId { get; set; }
        public string UserId { get; set; }
        public int NotificationId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool isRead { get; set; }
    }
}
