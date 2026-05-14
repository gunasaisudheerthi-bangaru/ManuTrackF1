import { Component, OnInit } from '@angular/core';
import { AnalyticsService } from '../../../core/services/analytics.service';
import { DashboardSummaryViewModel, KpiReportViewModel, ProductionMetricViewModel } from '../../../core/models/analytics.model';

@Component({
  selector: 'app-analytics-dashboard',
  templateUrl: './analytics-dashboard.component.html',
  standalone: false
})
export class AnalyticsDashboardComponent implements OnInit {
  dashboard: DashboardSummaryViewModel | null = null;
  reports: KpiReportViewModel[] = [];
  metrics: ProductionMetricViewModel[] = [];
  loading = false;
  errorMessage = '';
  selectedMetricType = '';
  selectedServiceSource = '';

  get kpiEntries(): { key: string; value: number }[] {
    if (!this.dashboard?.latestKpis) return [];
    return Object.entries(this.dashboard.latestKpis).map(([key, value]) => ({ key, value }));
  }

  constructor(private analyticsService: AnalyticsService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.errorMessage = '';

    this.analyticsService.getDashboard().subscribe({
      next: res => {
        if (res.success) this.dashboard = res.data;
      },
      error: () => {
        this.errorMessage = 'Analytics service unavailable. Please ensure all services are running.';
        this.loading = false;
      }
    });

    this.analyticsService.getReports().subscribe({
      next: res => { if (res.success) this.reports = res.data; },
      error: () => {}
    });

    this.analyticsService.getMetrics(
      this.selectedMetricType || undefined,
      this.selectedServiceSource || undefined
    ).subscribe({
      next: res => {
        this.loading = false;
        if (res.success) this.metrics = res.data;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  reloadMetrics(): void {
    this.analyticsService.getMetrics(
      this.selectedMetricType || undefined,
      this.selectedServiceSource || undefined
    ).subscribe({
      next: res => { if (res.success) this.metrics = res.data; },
      error: () => {}
    });
  }

  getValueClass(value: number): string {
    if (value >= 90) return 'text-success';
    if (value >= 70) return 'text-warning';
    return 'text-danger';
  }
}
