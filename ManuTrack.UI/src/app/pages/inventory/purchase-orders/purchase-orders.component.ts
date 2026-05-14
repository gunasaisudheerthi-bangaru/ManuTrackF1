import { Component, OnInit } from '@angular/core';
import { InventoryService } from '../../../core/services/inventory.service';
import { PurchaseOrderViewModel, CreatePurchaseOrderRequest } from '../../../core/models/inventory.model';

@Component({
  selector: 'app-purchase-orders',
  templateUrl: './purchase-orders.component.html',
  standalone: false
})
export class PurchaseOrdersComponent implements OnInit {
  purchaseOrders: PurchaseOrderViewModel[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';
  showForm = false;
  filterStatus = '';

  form: CreatePurchaseOrderRequest = {
    supplierID: '',
    supplierName: '',
    expectedDeliveryDate: '',
    notes: '',
    items: [{ productID: 0, quantity: 1, unitPrice: 0 }]
  };

  statuses = ['Draft', 'Submitted', 'Approved', 'Ordered', 'Received', 'Cancelled'];

  constructor(private inventoryService: InventoryService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.loading = true;
    this.inventoryService.getPurchaseOrders().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) this.purchaseOrders = res.data;
        else this.errorMessage = res.message;
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load purchase orders.'; }
    });
  }

  get filteredOrders(): PurchaseOrderViewModel[] {
    return this.filterStatus
      ? this.purchaseOrders.filter(o => o.status === this.filterStatus)
      : this.purchaseOrders;
  }

  addItem(): void {
    this.form.items.push({ productID: 0, quantity: 1, unitPrice: 0 });
  }

  removeItem(index: number): void {
    if (this.form.items.length > 1) this.form.items.splice(index, 1);
  }

  save(): void {
    this.inventoryService.createPurchaseOrder(this.form).subscribe({
      next: res => {
        if (res.success) {
          this.successMessage = 'Purchase order created successfully.';
          this.showForm = false;
          this.loadOrders();
          setTimeout(() => this.successMessage = '', 3000);
        } else { this.errorMessage = res.message; }
      },
      error: () => { this.errorMessage = 'Failed to create purchase order.'; }
    });
  }

  updateStatus(po: PurchaseOrderViewModel, status: string): void {
    this.inventoryService.updatePurchaseOrderStatus(po.pOID, status).subscribe({
      next: res => {
        if (res.success) po.status = status;
      },
      error: () => { this.errorMessage = 'Failed to update status.'; }
    });
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      Draft: 'bg-secondary', Submitted: 'bg-info text-dark', Approved: 'bg-primary',
      Ordered: 'bg-warning text-dark', Received: 'bg-success', Cancelled: 'bg-danger'
    };
    return map[status] || 'bg-dark';
  }
}
