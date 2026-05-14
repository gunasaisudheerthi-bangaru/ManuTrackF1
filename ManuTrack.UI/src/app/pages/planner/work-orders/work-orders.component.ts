import { Component, OnInit } from '@angular/core';
import { WorkOrderService } from '../../../core/services/work-order.service';
import { WorkOrderViewModel, CreateWorkOrderRequest } from '../../../core/models/work-order.model';
import { ProductService } from '../../../core/services/product.service';
import { ProductViewModel } from '../../../core/models/product.model';

@Component({
  selector: 'app-work-orders',
  templateUrl: './work-orders.component.html',
  standalone: false
})
export class WorkOrdersComponent implements OnInit {
  workOrders: WorkOrderViewModel[] = [];
  filteredOrders: WorkOrderViewModel[] = [];
  products: ProductViewModel[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';
  showForm = false;
  filterStatus = '';

  form: CreateWorkOrderRequest = {
    productID: 0, quantity: 1,
    startDate: '', endDate: '', notes: ''
  };

  statuses = ['Planned', 'InProgress', 'Completed', 'OnHold', 'Cancelled'];

  constructor(
    private workOrderService: WorkOrderService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.loadOrders();
    this.productService.getAll().subscribe(res => {
      if (res.success) this.products = res.data;
    });
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

  save(): void {
    this.workOrderService.create(this.form).subscribe({
      next: res => {
        if (res.success) {
          this.successMessage = 'Work order created successfully.';
          this.showForm = false;
          this.loadOrders();
          setTimeout(() => this.successMessage = '', 3000);
        } else { this.errorMessage = res.message; }
      },
      error: () => { this.errorMessage = 'Failed to create work order.'; }
    });
  }

  updateStatus(wo: WorkOrderViewModel, status: string): void {
    this.workOrderService.updateStatus(wo.workOrderID, status).subscribe({
      next: res => {
        if (res.success) { wo.status = status; }
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
}
