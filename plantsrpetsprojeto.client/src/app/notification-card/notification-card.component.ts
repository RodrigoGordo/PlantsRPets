import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../app/notification.service';
import { Notification } from '../models/notification.model';

@Component({
  standalone: false,
  selector: 'app-notification-card',
  templateUrl: './notification-card.component.html',
  styleUrls: ['./notification-card.component.css']
})

/**
 * Componente responsável por exibir e gerir as notificações do utilizador.
 * Permite visualizar, marcar como lidas e remover notificações.
 */
export class NotificationCardComponent implements OnInit {
  notifications: Notification[] = [];
  unreadNotifications: Notification[] = [];
  showNotifications = false;

  /**
   * Construtor do componente. Injeta o serviço de notificações.
   * 
   * @param notificationService - Serviço responsável por interagir com a API de notificações.
   */
  constructor(private notificationService: NotificationService) { }

  /**
   * Ciclo de vida do componente: inicializa a carga de notificações e não lidas.
   */
  ngOnInit(): void {
    this.loadNotifications();
    this.loadUnreadNotifications();
  }

  /**
   * Carrega todas as notificações do utilizador autenticado.
   */
  loadNotifications() {
    this.notificationService.getUserNotifications().subscribe((data: Notification[]) => {
      this.notifications = data;
      console.log(this.notifications);
    });
  }

  /**
   * Carrega apenas as notificações não lidas.
   */
  loadUnreadNotifications() {
    this.notificationService.getUnreadNotifications().subscribe((data: Notification[]) => {
      this.unreadNotifications = data;
    });
  }

  /**
   * Marca uma notificação como lida, atualizando a API e o estado local.
   * 
   * @param notification - Objeto de notificação a ser marcado como lido.
   */
  markAsRead(notification: Notification) {
    if (!notification.isRead) {
      this.notificationService.markAsRead(notification.userNotificationId).subscribe(() => {
        notification.isRead = true;
        this.loadUnreadNotifications();
      });
    }
  }

  /**
   * Remove uma notificação da lista, atualizando o estado local e a API.
   * 
   * @param notification - Objeto de notificação a ser eliminado.
   */
  deleteNotification(notification: Notification) {
    this.notificationService.deleteNotification(notification.userNotificationId).subscribe(() => {
      this.notifications = this.notifications.filter(n => n.userNotificationId !== notification.userNotificationId);
      this.loadUnreadNotifications();
    });
  }

  /**
   * Alterna a visibilidade da secção de notificações.
   */
  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
  }
}
