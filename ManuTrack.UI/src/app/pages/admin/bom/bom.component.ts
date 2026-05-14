import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../core/services/product.service';
import { BomViewModel, CreateBomRequest, ProductViewModel } from '../../../core/models/product.model';
import { ApiResponse } from '../../../core/models/notification.model';

@Component({
  selector: 'app-bom',
  templateUrl: './bom.component.html',
  standalone: false
})
export class BomComponent implements OnInit {
  boms: BomViewModel[] = [];
  products: ProductViewModel[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';
  showForm = false;
  selectedProductId: number | null = null;

  form: CreateBomRequest = {
    productID: 0,
    componentID: 0,
    quantity: 1,
    version: '',
    notes: ''
  };

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.productService.getAll().subscribe({
      next: (res: ApiResponse<ProductViewModel[]>) => {
        this.loading = false;
        if (res.success) this.products = res.data;
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Failed to load products.';
      }
    });
  }

  onProductChange(value: string): void {
    const id = parseInt(value, 10);
    this.selectedProductId = isNaN(id) ? null : id;
    this.boms = [];
    this.showForm = false;
    if (this.selectedProductId !== null) {
      this.loadBom(this.selectedProductId);
    }
  }

  loadBom(productId: number): void {
    this.loading = true;
    this.errorMessage = '';
    this.productService.getBom(productId).subscribe({
      next: (res: ApiResponse<BomViewModel[]>) => {
        this.loading = false;
        if (res.success) this.boms = res.data;
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Failed to load BOM.';
      }
    });
  }

  openAddForm(): void {
    if (this.selectedProductId === null) return;
    this.form = {
      productID: this.selectedProductId,
      componentID: 0,
      quantity: 1,
      version: '',
      notes: ''
    };
    this.showForm = true;
    this.errorMessage = '';
  }

  save(): void {
    if (!this.form.componentID || this.form.quantity < 1) {
      this.errorMessage = 'Component ID and a valid quantity are required.';
      return;
    }

    this.form.productID = this.selectedProductId!;
    this.productService.createBom(this.form).subscribe({
      next: () => {
        this.successMessage = 'BOM item added successfully.';
        this.showForm = false;
        this.errorMessage = '';
        this.loadBom(this.selectedProductId!);
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to add BOM item.';
      }
    });
  }
}
