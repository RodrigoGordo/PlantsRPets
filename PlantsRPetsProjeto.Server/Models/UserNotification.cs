namespace PlantsRPetsProjeto.Server.Models
{
    public class UserNotification
    {
        public int UserNotificationId { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual Notification Notification { get; set; }
        public int NotificationId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool isRead { get; set; }
    }
}
