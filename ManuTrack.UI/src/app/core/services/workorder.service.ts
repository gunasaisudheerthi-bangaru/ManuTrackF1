import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from './product.service';

export interface WorkOrderViewModel {
  workOrderID: number;
  productID: number;
  productName: string;
  quantity: number;
  startDate: string;
  endDate: string;
  status: string;
  taskCount: number;
  progressPercentage: number;
  isOverdue: boolean;
  createdDate: string;
}

export interface WorkOrderTaskViewModel {
  taskID: number;
  workOrderID: number;
  description: string;
  assignedTo: string;
  status: string;
  notes?: string;
  completedDate?: string;
  createdDate: string;
}

export interface CreateWorkOrderRequest {
  productID: number;
  productName: string;
  quantity: number;
  startDate: string;
  endDate: string;
}

export interface CreateTaskRequest {
  workOrderID: number;
  description: string;
  assignedTo: string;
  notes?: string;
}

@Injectable({ providedIn: 'root' })
export class WorkOrderService {
  private readonly base = 'http://localhost:5000/api/v1/workorders';
  private readonly taskBase = 'http://localhost:5000/api/v1/tasks';

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApiResponse<WorkOrderViewModel[]>> {
    return this.http.get<ApiResponse<WorkOrderViewModel[]>>(this.base);
  }

  create(req: CreateWorkOrderRequest): Observable<ApiResponse<WorkOrderViewModel>> {
    return this.http.post<ApiResponse<WorkOrderViewModel>>(this.base, req);
  }

  updateStatus(id: number, status: string): Observable<ApiResponse<WorkOrderViewModel>> {
    return this.http.put<ApiResponse<WorkOrderViewModel>>(`${this.base}/${id}/status`, { status });
  }

  delete(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.base}/${id}`);
  }

  // Tasks
  getTasksByWorkOrder(workOrderId: number): Observable<ApiResponse<WorkOrderTaskViewModel[]>> {
    return this.http.get<ApiResponse<WorkOrderTaskViewModel[]>>(`${this.taskBase}/workorder/${workOrderId}`);
  }

  createTask(req: CreateTaskRequest): Observable<ApiResponse<WorkOrderTaskViewModel>> {
    return this.http.post<ApiResponse<WorkOrderTaskViewModel>>(this.taskBase, req);
  }

  updateTaskStatus(taskId: number, status: string): Observable<ApiResponse<WorkOrderTaskViewModel>> {
    return this.http.put<ApiResponse<WorkOrderTaskViewModel>>(`${this.taskBase}/${taskId}/status`, { status });
  }

  deleteTask(taskId: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.taskBase}/${taskId}`);
  }
}
