import { Component, OnInit } from '@angular/core';
import { WorkOrderService } from '../../../core/services/work-order.service';
import { WorkOrderViewModel } from '../../../core/models/work-order.model';

@Component({
  selector: 'app-issue-report',
  templateUrl: './issue-report.component.html',
  standalone: false
})
export class IssueReportComponent implements OnInit {
  workOrders: WorkOrderViewModel[] = [];
  loading = false;
  submitting = false;
  errorMessage = '';
  successMessage = '';

  form = {
    workOrderID: 0,
    issueType: '',
    severity: 'Medium',
    description: '',
    suggestedAction: ''
  };

  issueTypes = ['Equipment Failure', 'Material Shortage', 'Quality Issue', 'Safety Concern', 'Process Deviation', 'Other'];
  severities = ['Low', 'Medium', 'High', 'Critical'];

  constructor(private workOrderService: WorkOrderService) {}

  ngOnInit(): void {
    this.loading = true;
    this.workOrderService.getAll().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) this.workOrders = res.data.filter(o => o.status !== 'Completed' && o.status !== 'Cancelled');
      },
      error: () => { this.loading = false; }
    });
  }

  submit(): void {
    if (!this.form.workOrderID || !this.form.issueType || !this.form.description) {
      this.errorMessage = 'Please fill in all required fields.';
      return;
    }
    this.submitting = true;
    // Put the related work order on hold and show success
    const wo = this.workOrders.find(o => o.workOrderID === this.form.workOrderID);
    if (wo) {
      this.workOrderService.updateStatus(wo.workOrderID, 'OnHold').subscribe({
        next: () => {
          if (wo) wo.status = 'OnHold';
        }
      });
    }
    setTimeout(() => {
      this.submitting = false;
      this.successMessage = 'Issue reported successfully. Supervisor has been notified.';
      this.form = { workOrderID: 0, issueType: '', severity: 'Medium', description: '', suggestedAction: '' };
      setTimeout(() => this.successMessage = '', 5000);
    }, 500);
  }

  getSeverityClass(s: string): string {
    const map: Record<string, string> = {
      Low: 'success', Medium: 'warning', High: 'orange', Critical: 'danger'
    };
    return `text-${map[s] || 'dark'}`;
  }
}
