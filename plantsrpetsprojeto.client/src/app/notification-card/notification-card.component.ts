import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../app/notification.service';
import { Notification } from '../models/notification.model';

@Component({
  standalone: false,
  selector: 'app-notification-card',
  templateUrl: './notification-card.component.html',
  styleUrls: ['./notification-card.component.css']
})
export class NotificationCardComponent implements OnInit {
  notifications: Notification[] = [];
  unreadNotifications: Notification[] = [];
  showNotifications = false;

  constructor(private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.loadNotifications();
    this.loadUnreadNotifications();
  }

  loadNotifications() {
    this.notificationService.getUserNotifications().subscribe((data: Notification[]) => {
      this.notifications = data;
      console.log(this.notifications);
    });
  }

  loadUnreadNotifications() {
    this.notificationService.getUnreadNotifications().subscribe((data: Notification[]) => {
      this.unreadNotifications = data;
    });
  }

  markAsRead(notification: Notification) {
    if (!notification.isRead) {
      this.notificationService.markAsRead(notification.userNotificationId).subscribe(() => {
        notification.isRead = true;
        this.loadUnreadNotifications();
      });
    }
  }

  deleteNotification(notification: Notification) {
    this.notificationService.deleteNotification(notification.userNotificationId).subscribe(() => {
      this.notifications = this.notifications.filter(n => n.userNotificationId !== notification.userNotificationId);
      this.loadUnreadNotifications();
    });
  }

  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
  }
}
