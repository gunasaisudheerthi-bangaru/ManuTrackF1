import { Component, OnInit } from '@angular/core';
import { WorkOrderService } from '../../../core/services/work-order.service';
import { WorkOrderViewModel } from '../../../core/models/work-order.model';

@Component({
  selector: 'app-progress-update',
  templateUrl: './progress-update.component.html',
  standalone: false
})
export class ProgressUpdateComponent implements OnInit {
  workOrders: WorkOrderViewModel[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';
  selectedOrder: WorkOrderViewModel | null = null;
  selectedStatus = '';

  statuses = ['Planned', 'InProgress', 'Completed', 'OnHold', 'Cancelled'];

  constructor(private workOrderService: WorkOrderService) {}

  ngOnInit(): void {
    this.loading = true;
    this.workOrderService.getAll().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) this.workOrders = res.data.filter(o => o.status !== 'Completed' && o.status !== 'Cancelled');
        else this.errorMessage = res.message;
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load work orders.'; }
    });
  }

  selectOrder(wo: WorkOrderViewModel): void {
    this.selectedOrder = wo;
    this.selectedStatus = wo.status;
    this.errorMessage = '';
  }

  submit(): void {
    if (!this.selectedOrder) return;
    this.workOrderService.updateStatus(this.selectedOrder.workOrderID, this.selectedStatus).subscribe({
      next: res => {
        if (res.success) {
          this.successMessage = 'Status updated successfully.';
          if (this.selectedOrder) {
            this.selectedOrder.status = this.selectedStatus;
            if (this.selectedStatus === 'Completed' || this.selectedStatus === 'Cancelled') {
              this.workOrders = this.workOrders.filter(o => o.workOrderID !== this.selectedOrder!.workOrderID);
            }
          }
          this.selectedOrder = null;
          setTimeout(() => this.successMessage = '', 3000);
        } else { this.errorMessage = res.message; }
      },
      error: () => { this.errorMessage = 'Failed to update status.'; }
    });
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      Planned: 'bg-secondary', InProgress: 'bg-primary',
      Completed: 'bg-success', OnHold: 'bg-warning text-dark', Cancelled: 'bg-danger'
    };
    return map[status] || 'bg-dark';
  }
}
