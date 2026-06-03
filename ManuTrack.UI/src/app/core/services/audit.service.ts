import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from './product.service';

export interface AuditEntryViewModel {
  auditID: number;
  userID: number;
  userName: string;
  action: string;
  entityType: string;
  entityID: string;
  serviceName: string;
  details?: string;
  timestamp: string;
}

export interface PagedAuditViewModel {
  data: AuditEntryViewModel[];
  pagination: { currentPage: number; pageSize: number; totalRecords: number; totalPages: number };
}

@Injectable({ providedIn: 'root' })
export class AuditService {
  private readonly base = 'http://localhost:5000/api/v1/audit-logs';

  constructor(private http: HttpClient) {}

  getAll(page = 1, pageSize = 20, action?: string): Observable<ApiResponse<PagedAuditViewModel>> {
    let url = `${this.base}?page=${page}&pageSize=${pageSize}`;
    if (action) url += `&action=${action}`;
    return this.http.get<ApiResponse<PagedAuditViewModel>>(url);
  }
}
