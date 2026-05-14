import { Component, OnInit } from '@angular/core';
import { QualityService } from '../../../core/services/quality.service';
import { InspectionViewModel, CreateInspectionRequest } from '../../../core/models/quality.model';
import { WorkOrderService } from '../../../core/services/work-order.service';
import { WorkOrderViewModel } from '../../../core/models/work-order.model';

@Component({
  selector: 'app-inspections',
  templateUrl: './inspections.component.html',
  standalone: false
})
export class InspectionsComponent implements OnInit {
  inspections: InspectionViewModel[] = [];
  workOrders: WorkOrderViewModel[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';
  showForm = false;

  form: CreateInspectionRequest = {
    workOrderID: 0,
    notes: ''
  };

  results = ['Pass', 'Fail', 'ConditionalPass'];

  constructor(
    private qualityService: QualityService,
    private workOrderService: WorkOrderService
  ) {}

  ngOnInit(): void {
    this.loadInspections();
    this.workOrderService.getAll().subscribe({
      next: res => { if (res.success) this.workOrders = res.data; },
      error: () => {}
    });
  }

  loadInspections(): void {
    this.loading = true;
    this.qualityService.getInspections().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) this.inspections = res.data;
        else this.errorMessage = res.message;
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load inspections.'; }
    });
  }

  save(): void {
    this.qualityService.createInspection(this.form).subscribe({
      next: res => {
        if (res.success) {
          this.successMessage = 'Inspection created successfully.';
          this.showForm = false;
          this.loadInspections();
          setTimeout(() => this.successMessage = '', 3000);
        } else { this.errorMessage = res.message; }
      },
      error: () => { this.errorMessage = 'Failed to create inspection.'; }
    });
  }

  submitResult(id: number, result: string): void {
    this.qualityService.submitResult(id, result, '').subscribe({
      next: res => {
        if (res.success) {
          const insp = this.inspections.find(i => i.inspectionID === id);
          if (insp) insp.result = result;
        }
      },
      error: () => { this.errorMessage = 'Failed to submit result.'; }
    });
  }

  getResultClass(result: string): string {
    const map: Record<string, string> = {
      Pass: 'bg-success', Fail: 'bg-danger', ConditionalPass: 'bg-warning text-dark', Pending: 'bg-secondary'
    };
    return map[result] || 'bg-secondary';
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      Pending: 'bg-secondary', InProgress: 'bg-info text-dark', Completed: 'bg-success'
    };
    return map[status] || 'bg-secondary';
  }

  getWoNumber(id: number): string {
    const wo = this.workOrders.find(w => w.workOrderID === id);
    return wo ? `#${wo.workOrderID}` : `#${id}`;
  }
}
