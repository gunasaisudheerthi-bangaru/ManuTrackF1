import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { NotificationViewModel, SendNotificationRequest, BroadcastNotificationRequest, ApiResponse } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private url = `${environment.apiUrl}/notifications`;

  constructor(private http: HttpClient) {}

  getMyNotifications(): Observable<ApiResponse<NotificationViewModel[]>> {
    return this.http.get<ApiResponse<NotificationViewModel[]>>(`${this.url}/my`);
  }

  getUnreadCount(): Observable<ApiResponse<number>> {
    return this.http.get<ApiResponse<number>>(`${this.url}/my/unread-count`);
  }

  markAsRead(id: number): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.url}/${id}/read`, {});
  }

  markAllAsRead(): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.url}/my/read-all`, {});
  }

  send(request: SendNotificationRequest): Observable<ApiResponse<NotificationViewModel>> {
    return this.http.post<ApiResponse<NotificationViewModel>>(`${this.url}/send`, request);
  }

  broadcast(request: BroadcastNotificationRequest): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.url}/broadcast`, request);
  }
}
