import { Component, OnInit } from '@angular/core';
import { InventoryService } from '../../../core/services/inventory.service';
import { InventoryViewModel } from '../../../core/models/inventory.model';

@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  standalone: false
})
export class StockComponent implements OnInit {
  inventory: InventoryViewModel[] = [];
  filteredInventory: InventoryViewModel[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';
  searchTerm = '';
  showAdjustForm = false;
  selectedItem: InventoryViewModel | null = null;

  adjustForm = {
    quantity: 0,
    reason: ''
  };

  constructor(private inventoryService: InventoryService) {}

  ngOnInit(): void {
    this.loadInventory();
  }

  loadInventory(): void {
    this.loading = true;
    this.inventoryService.getAll().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) { this.inventory = res.data; this.applyFilter(); }
        else this.errorMessage = res.message;
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load inventory.'; }
    });
  }

  applyFilter(): void {
    this.filteredInventory = this.searchTerm
      ? this.inventory.filter(i =>
          i.productName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
          i.locationID.toLowerCase().includes(this.searchTerm.toLowerCase()))
      : [...this.inventory];
  }

  openAdjust(item: InventoryViewModel): void {
    this.selectedItem = item;
    this.adjustForm = { quantity: 0, reason: '' };
    this.showAdjustForm = true;
  }

  submitAdjust(): void {
    if (!this.selectedItem) return;
    this.inventoryService.adjustQuantity(this.selectedItem.inventoryID, this.adjustForm.quantity, this.adjustForm.reason).subscribe({
      next: res => {
        if (res.success) {
          this.successMessage = 'Inventory adjusted successfully.';
          this.showAdjustForm = false;
          this.loadInventory();
          setTimeout(() => this.successMessage = '', 3000);
        } else { this.errorMessage = res.message; }
      },
      error: () => { this.errorMessage = 'Failed to adjust inventory.'; }
    });
  }

  getStockClass(item: InventoryViewModel): string {
    if (item.status === 'OutOfStock') return 'text-danger fw-bold';
    if (item.status === 'LowStock') return 'text-warning fw-bold';
    return 'text-success';
  }

  getStockBadge(item: InventoryViewModel): string {
    if (item.status === 'OutOfStock') return 'bg-danger';
    if (item.status === 'LowStock') return 'bg-warning text-dark';
    return 'bg-success';
  }

  getStockLabel(item: InventoryViewModel): string {
    if (item.status === 'OutOfStock') return 'Out of Stock';
    if (item.status === 'LowStock') return 'Low Stock';
    return 'Available';
  }

  get lowStockCount(): number { return this.inventory.filter(i => i.status === 'LowStock').length; }
  get outOfStockCount(): number { return this.inventory.filter(i => i.status === 'OutOfStock').length; }
}
