import { Component, OnInit } from '@angular/core';
import { WorkOrderService } from '../../../core/services/work-order.service';
import { WorkOrderViewModel } from '../../../core/models/work-order.model';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  standalone: false
})
export class TasksComponent implements OnInit {
  workOrders: WorkOrderViewModel[] = [];
  filteredOrders: WorkOrderViewModel[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';
  filterStatus = '';

  statuses = ['Planned', 'InProgress', 'Completed', 'OnHold', 'Cancelled'];

  constructor(private workOrderService: WorkOrderService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.loading = true;
    this.workOrderService.getAll().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) { this.workOrders = res.data; this.applyFilter(); }
        else this.errorMessage = res.message;
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load work orders.'; }
    });
  }

  applyFilter(): void {
    this.filteredOrders = this.filterStatus
      ? this.workOrders.filter(o => o.status === this.filterStatus)
      : [...this.workOrders];
  }

  updateStatus(wo: WorkOrderViewModel, status: string): void {
    this.workOrderService.updateStatus(wo.workOrderID, status).subscribe({
      next: res => {
        if (res.success) {
          wo.status = status;
          this.successMessage = 'Status updated.';
          setTimeout(() => this.successMessage = '', 2000);
        }
      }
    });
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      Planned: 'bg-secondary', InProgress: 'bg-primary',
      Completed: 'bg-success', OnHold: 'bg-warning text-dark', Cancelled: 'bg-danger'
    };
    return map[status] || 'bg-dark';
  }

  getPlannedCount(): number { return this.workOrders.filter(o => o.status === 'Planned').length; }
  getInProgressCount(): number { return this.workOrders.filter(o => o.status === 'InProgress').length; }
  getCompletedCount(): number { return this.workOrders.filter(o => o.status === 'Completed').length; }
}
