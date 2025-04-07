import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Notification } from '../app/models/notification.model';

@Injectable({
  providedIn: 'root'
})

/**
 * Serviço responsável pela gestão de notificações do utilizador.
 * Permite ler, marcar como lida, eliminar e configurar frequência de emails.
 */
export class NotificationService {
  private apiUrl = 'api/notification';

  constructor(private http: HttpClient) { }

  /**
   * Obtém todas as notificações associadas ao utilizador atual.
   * @returns Observable com array de notificações
   */
  getUserNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}`);
  }

  /**
   * Obtém apenas as notificações ainda não lidas.
   * @returns Observable com array de notificações por ler
   */
  getUnreadNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}/unread`);
  }

  /**
   * Envia uma notificação manualmente para o utilizador.
   * @param notificationId ID da notificação a enviar
   * @returns Observable com o resultado da operação
   */
  sendNotification(notificationId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/send/${notificationId}`, {});
  }

  /**
   * Marca uma notificação como lida.
   * @param notificationId ID da notificação
   * @returns Observable com o resultado da operação
   */
  markAsRead(notificationId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/read/${notificationId}`, {});
  }

  /**
   * Remove uma notificação específica da base de dados do utilizador.
   * @param notificationId ID da notificação a eliminar
   * @returns Observable com o resultado da operação
   */
  deleteNotification(notificationId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${notificationId}`);
  }

  /**
   * Atualiza a frequência com que o utilizador recebe notificações por e-mail.
   * @param frequencyId Valor numérico correspondente à enumeração (0: Nunca, 1: Diário, etc.)
   * @returns Observable com o resultado da atualização
   */
  updateEmailFrequency(frequencyId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/email-frequency/${frequencyId}`, {});
  }
}
