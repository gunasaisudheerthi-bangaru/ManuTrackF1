import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from './product.service';

export interface DashboardSummaryViewModel {
  totalReports: number;
  totalMetrics: number;
  activeWorkOrders: number;
  overdueWorkOrders: number;
  completedThisMonth: number;
  lowStockItems: number;
  outOfStockItems: number;
  latestKpis: Record<string, number>;
}

export interface KpiReportViewModel {
  reportID: number;
  title: string;
  reportType: string;
  scope: string;
  generatedBy: string;
  generatedDate: string;
  metrics: string;
}

export interface ProductionMetricViewModel {
  metricID: number;
  metricType: string;
  metricName: string;
  value: number;
  unit?: string;
  serviceSource?: string;
  recordedDate: string;
}

export interface CreateKpiReportRequest {
  title: string;
  reportType: string;
  scope: string;
  generatedBy: string;
  metrics?: string;
  periodStart?: string;
  periodEnd?: string;
}

export interface RecordMetricRequest {
  metricType: string;
  metricName: string;
  value: number;
  unit?: string;
  serviceSource?: string;
}

@Injectable({ providedIn: 'root' })
export class AnalyticsService {
  private readonly base = '/api/v1/analytics';

  constructor(private http: HttpClient) {}

  getDashboard(): Observable<ApiResponse<DashboardSummaryViewModel>> {
    return this.http.get<ApiResponse<DashboardSummaryViewModel>>(`${this.base}/dashboard`);
  }

  getReports(): Observable<ApiResponse<KpiReportViewModel[]>> {
    return this.http.get<ApiResponse<KpiReportViewModel[]>>(`${this.base}/reports`);
  }

  createReport(req: CreateKpiReportRequest): Observable<ApiResponse<KpiReportViewModel>> {
    return this.http.post<ApiResponse<KpiReportViewModel>>(`${this.base}/reports`, req);
  }

  getMetrics(): Observable<ApiResponse<{ data: ProductionMetricViewModel[] }>> {
    return this.http.get<ApiResponse<{ data: ProductionMetricViewModel[] }>>(`${this.base}/metrics`);
  }

  recordMetric(req: RecordMetricRequest): Observable<ApiResponse<ProductionMetricViewModel>> {
    return this.http.post<ApiResponse<ProductionMetricViewModel>>(`${this.base}/metrics`, req);
  }
}
