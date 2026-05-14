import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  WorkOrderViewModel, CreateWorkOrderRequest,
  WorkOrderTaskViewModel, CreateWorkOrderTaskRequest, UpdateTaskStatusRequest
} from '../models/work-order.model';
import { ApiResponse } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class WorkOrderService {
  private url = `${environment.apiUrl}/workorders`;
  private taskUrl = `${environment.apiUrl}/tasks`;

  constructor(private http: HttpClient) {}

  // ── Work Orders ──────────────────────────────────────────────────────────
  getAll(status?: string): Observable<ApiResponse<WorkOrderViewModel[]>> {
    const params = status ? `?status=${status}` : '';
    return this.http.get<ApiResponse<WorkOrderViewModel[]>>(`${this.url}${params}`);
  }

  getById(id: number): Observable<ApiResponse<WorkOrderViewModel>> {
    return this.http.get<ApiResponse<WorkOrderViewModel>>(`${this.url}/${id}`);
  }

  create(request: CreateWorkOrderRequest): Observable<ApiResponse<WorkOrderViewModel>> {
    return this.http.post<ApiResponse<WorkOrderViewModel>>(this.url, request);
  }

  updateStatus(id: number, status: string): Observable<ApiResponse<WorkOrderViewModel>> {
    return this.http.put<ApiResponse<WorkOrderViewModel>>(`${this.url}/${id}/status`, { status });
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.url}/${id}`);
  }

  // ── Tasks ────────────────────────────────────────────────────────────────
  getTasksByWorkOrder(workOrderId: number): Observable<ApiResponse<WorkOrderTaskViewModel[]>> {
    return this.http.get<ApiResponse<WorkOrderTaskViewModel[]>>(`${this.taskUrl}/workorder/${workOrderId}`);
  }

  getTaskById(id: number): Observable<ApiResponse<WorkOrderTaskViewModel>> {
    return this.http.get<ApiResponse<WorkOrderTaskViewModel>>(`${this.taskUrl}/${id}`);
  }

  createTask(request: CreateWorkOrderTaskRequest): Observable<ApiResponse<WorkOrderTaskViewModel>> {
    return this.http.post<ApiResponse<WorkOrderTaskViewModel>>(this.taskUrl, request);
  }

  updateTaskStatus(taskId: number, status: string): Observable<ApiResponse<WorkOrderTaskViewModel>> {
    return this.http.put<ApiResponse<WorkOrderTaskViewModel>>(`${this.taskUrl}/${taskId}/status`, { status });
  }
}
