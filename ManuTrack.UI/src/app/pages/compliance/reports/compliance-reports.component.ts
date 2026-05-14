import { Component, OnInit } from '@angular/core';
import { ComplianceService } from '../../../core/services/compliance.service';
import { ComplianceReportViewModel, CreateComplianceReportRequest } from '../../../core/models/compliance.model';

@Component({
  selector: 'app-compliance-reports',
  templateUrl: './compliance-reports.component.html',
  standalone: false
})
export class ComplianceReportsComponent implements OnInit {
  reports: ComplianceReportViewModel[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';
  showForm = false;

  form: CreateComplianceReportRequest = {
    reportType: '',
    scope: '',
    periodStart: '',
    periodEnd: ''
  };

  reportTypes = ['ISO 9001', 'ISO 14001', 'OSHA', 'GMP', 'SOX', 'Internal Audit', 'Regulatory'];

  constructor(private complianceService: ComplianceService) {}

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    this.loading = true;
    this.complianceService.getReports().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) this.reports = res.data;
        else this.errorMessage = res.message;
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load reports.'; }
    });
  }

  save(): void {
    this.complianceService.createReport(this.form).subscribe({
      next: res => {
        if (res.success) {
          this.successMessage = 'Compliance report created successfully.';
          this.showForm = false;
          this.loadReports();
          setTimeout(() => this.successMessage = '', 3000);
        } else { this.errorMessage = res.message; }
      },
      error: () => { this.errorMessage = 'Failed to create report.'; }
    });
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      Draft: 'bg-secondary', UnderReview: 'bg-warning text-dark',
      Approved: 'bg-success', Rejected: 'bg-danger', Published: 'bg-primary'
    };
    return map[status] || 'bg-dark';
  }
}
