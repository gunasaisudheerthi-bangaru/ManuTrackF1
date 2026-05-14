import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ComplianceReportViewModel, CreateComplianceReportRequest,
  AuditLogViewModel
} from '../models/compliance.model';
import { ApiResponse } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class ComplianceService {
  private reportUrl = `${environment.apiUrl}/compliance`;
  private auditUrl = `${environment.apiUrl}/audit-logs`;

  constructor(private http: HttpClient) {}

  getReports(): Observable<ApiResponse<ComplianceReportViewModel[]>> {
    return this.http.get<ApiResponse<ComplianceReportViewModel[]>>(this.reportUrl);
  }

  createReport(request: CreateComplianceReportRequest): Observable<ApiResponse<ComplianceReportViewModel>> {
    return this.http.post<ApiResponse<ComplianceReportViewModel>>(this.reportUrl, request);
  }

  getAuditLogs(): Observable<ApiResponse<AuditLogViewModel[]>> {
    return this.http.get<ApiResponse<AuditLogViewModel[]>>(this.auditUrl);
  }
}
