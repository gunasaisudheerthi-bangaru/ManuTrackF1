import { Component, OnInit } from '@angular/core';
import { AnalyticsService } from '../../../core/services/analytics.service';
import { KpiReportViewModel } from '../../../core/models/analytics.model';

@Component({
  selector: 'app-quality-reports',
  templateUrl: './quality-reports.component.html',
  standalone: false
})
export class QualityReportsComponent implements OnInit {
  reports: KpiReportViewModel[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';
  showForm = false;

  form = {
    reportType: '',
    scope: '',
    periodStart: '',
    periodEnd: ''
  };

  reportTypes = ['Weekly Quality Summary', 'Defect Analysis', 'Inspection Summary', 'Non-Conformance Report'];

  constructor(private analyticsService: AnalyticsService) {}

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    this.loading = true;
    this.analyticsService.getReports().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) this.reports = res.data;
        else this.errorMessage = res.message;
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load reports.'; }
    });
  }

  generate(): void {
    this.analyticsService.generateReport({
      reportType: this.form.reportType,
      scope: this.form.scope,
      periodStart: this.form.periodStart || undefined,
      periodEnd: this.form.periodEnd || undefined
    }).subscribe({
      next: res => {
        if (res.success) {
          this.successMessage = 'Report generated successfully.';
          this.showForm = false;
          this.loadReports();
          setTimeout(() => this.successMessage = '', 3000);
        } else { this.errorMessage = res.message; }
      },
      error: () => { this.errorMessage = 'Failed to generate report.'; }
    });
  }
}
