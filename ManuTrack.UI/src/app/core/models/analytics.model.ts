// Matches KpiReportViewModel.cs in AnalyticsService
export interface KpiReportViewModel {
  reportID: number;
  title: string;
  reportType: string;
  scope: string;
  metrics: string;          // JSON string of metric data
  generatedDate: string;
  generatedBy: string;
  periodStart?: string;
  periodEnd?: string;
}

// Matches ProductionMetricViewModel.cs in AnalyticsService
export interface ProductionMetricViewModel {
  metricID: number;
  metricType: string;
  metricName: string;
  value: number;
  unit: string;
  serviceSource?: string;
  entityID?: string;
  recordedDate: string;
}

// Matches DashboardSummaryViewModel.cs in AnalyticsService
export interface DashboardSummaryViewModel {
  totalReports: number;
  totalMetrics: number;
  latestKpis: { [key: string]: number };
}
