/**
 * Representa uma notificação recebida por um utilizador.
 */
export interface Notification {
  isRead: boolean;
  notification: {
    notificationId: number;
    type: string;
    message: string;
  };
  userNotificationId: number
}
