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
  showNotificationsSettings = false;
  selectedFrequency!: number;
  frequencyOptions = ['Never', 'Daily', 'Weekly', 'Monthly'];

  constructor(private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.loadNotifications();
    this.loadUnreadNotifications();
    this.getEmailFrequency();
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
    this.showNotificationsSettings = false;
  }

  toggleNotificationsSettings() {
    this.showNotificationsSettings = !this.showNotificationsSettings;
  }

  setEmailFrequency(frequency: number) {
    this.notificationService.updateEmailFrequency(frequency).subscribe({
      next: () => {
        this.selectedFrequency = frequency;
        console.log('Frequency updated to:', frequency);
      },
      error: (err) => {
        console.error('Update failed:', err);
        this.selectedFrequency = frequency;
      }
    });
  }

  getEmailFrequency() {
    this.notificationService.getEmailFrequency().subscribe(
      (data) => {
        this.selectedFrequency = data;
        console.log(this.selectedFrequency);
      }, (error) => {
        console.log('Get Email Failed:', error);
      }
    );
  }
}
