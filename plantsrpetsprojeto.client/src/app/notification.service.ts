import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Notification } from '../app/models/notification.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = 'api/notification';

  constructor(private http: HttpClient) { }

  // Get all notifications for the current user
  getUserNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}`);
  }

  // Get unread notifications
  getUnreadNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}/unread`);
  }

  // Send a specific notification to the user
  sendNotification(notificationId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/send/${notificationId}`, {});
  }

  // Mark a notification as read
  markAsRead(notificationId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/read/${notificationId}`, {});
  }

  // Delete a specific notification
  deleteNotification(notificationId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${notificationId}`);
  }

  // Update email frequency (Never, Daily, Weekly, Monthly)
  updateEmailFrequency(frequencyId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/email-frequency/${frequencyId}`, {});
  }

  getEmailFrequency(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/email-frequency`);
  }
}
