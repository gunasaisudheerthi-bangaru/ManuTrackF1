import { Component, OnInit } from '@angular/core';
import { WorkOrderService } from '../../../core/services/work-order.service';
import { WorkOrderViewModel } from '../../../core/models/work-order.model';

@Component({
  selector: 'app-progress',
  templateUrl: './progress.component.html',
  standalone: false
})
export class ProgressComponent implements OnInit {
  workOrders: WorkOrderViewModel[] = [];
  loading = false;
  errorMessage = '';

  get totalOrders() { return this.workOrders.length; }
  get completedOrders() { return this.workOrders.filter(o => o.status === 'Completed').length; }
  get inProgressOrders() { return this.workOrders.filter(o => o.status === 'InProgress').length; }
  get plannedOrders() { return this.workOrders.filter(o => o.status === 'Planned').length; }
  get overallProgress() {
    return this.totalOrders > 0 ? Math.round((this.completedOrders / this.totalOrders) * 100) : 0;
  }

  constructor(private workOrderService: WorkOrderService) {}

  ngOnInit(): void {
    this.loading = true;
    this.workOrderService.getAll().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) this.workOrders = res.data;
        else this.errorMessage = res.message;
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load data.'; }
    });
  }

  getStatusProgress(wo: WorkOrderViewModel): number {
    switch (wo.status) {
      case 'Completed': return 100;
      case 'InProgress': return 50;
      case 'OnHold': return 25;
      case 'Planned': return 0;
      default: return 0;
    }
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      Planned: 'bg-secondary', InProgress: 'bg-primary',
      Completed: 'bg-success', OnHold: 'bg-warning text-dark', Cancelled: 'bg-danger'
    };
    return map[status] || 'bg-dark';
  }
}
