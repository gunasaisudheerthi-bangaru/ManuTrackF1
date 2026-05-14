import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { KpiReportViewModel, ProductionMetricViewModel, DashboardSummaryViewModel } from '../models/analytics.model';
import { ApiResponse } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class AnalyticsService {
  private url = `${environment.apiUrl}/analytics`;

  constructor(private http: HttpClient) {}

  getDashboard(): Observable<ApiResponse<DashboardSummaryViewModel>> {
    return this.http.get<ApiResponse<DashboardSummaryViewModel>>(`${this.url}/dashboard`);
  }

  getReports(reportType?: string): Observable<ApiResponse<KpiReportViewModel[]>> {
    const params = reportType ? `?reportType=${reportType}` : '';
    return this.http.get<ApiResponse<KpiReportViewModel[]>>(`${this.url}/reports${params}`);
  }

  generateReport(request: { reportType: string; scope: string; periodStart?: string; periodEnd?: string }): Observable<ApiResponse<KpiReportViewModel>> {
    return this.http.post<ApiResponse<KpiReportViewModel>>(`${this.url}/reports`, request);
  }

  getMetrics(metricType?: string, serviceSource?: string): Observable<ApiResponse<ProductionMetricViewModel[]>> {
    const params = new URLSearchParams();
    if (metricType) params.set('metricType', metricType);
    if (serviceSource) params.set('serviceSource', serviceSource);
    const query = params.toString() ? `?${params}` : '';
    return this.http.get<ApiResponse<ProductionMetricViewModel[]>>(`${this.url}/metrics${query}`);
  }
}
