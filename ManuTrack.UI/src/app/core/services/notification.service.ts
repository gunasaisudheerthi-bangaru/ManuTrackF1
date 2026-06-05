import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from './product.service';

export interface NotificationViewModel {
  notificationID: number;
  userID: number;
  title: string;
  message: string;
  category: string;
  status: string;
  priority: string;
  createdDate: string;
  readDate?: string;
}

export interface BroadcastNotificationRequest {
  userIDs: number[];
  title: string;
  message: string;
  category: string;
}

@Injectable({ providedIn: 'root' })
export class NotificationAdminService {
  private readonly base = '/api/v1/notifications';

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApiResponse<NotificationViewModel[]>> {
    return this.http.get<ApiResponse<NotificationViewModel[]>>(this.base);
  }

  broadcast(req: BroadcastNotificationRequest): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.base}/broadcast`, req);
  }

  cleanup(): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.base}/cleanup`);
  }
}
