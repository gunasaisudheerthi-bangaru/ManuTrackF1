import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../core/services/product.service';
import { ProductViewModel, CreateProductRequest } from '../../../core/models/product.model';
import { ApiResponse } from '../../../core/models/notification.model';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  standalone: false
})
export class ProductsComponent implements OnInit {
  products: ProductViewModel[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';
  showForm = false;
  editMode = false;
  selectedProduct: ProductViewModel | null = null;

  form: CreateProductRequest = {
    name: '',
    category: '',
    version: '1.0',
    description: ''
  };

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.errorMessage = '';
    this.productService.getAll().subscribe({
      next: (res: ApiResponse<ProductViewModel[]>) => {
        this.loading = false;
        if (res.success) this.products = res.data;
        else this.errorMessage = res.message;
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Failed to load products.';
      }
    });
  }

  openCreate(): void {
    this.editMode = false;
    this.selectedProduct = null;
    this.form = { name: '', category: '', version: '1.0', description: '' };
    this.showForm = true;
    this.errorMessage = '';
  }

  openEdit(p: ProductViewModel): void {
    this.editMode = true;
    this.selectedProduct = p;
    this.form = {
      name: p.name,
      category: p.category,
      version: p.version,
      description: p.description || ''
    };
    this.showForm = true;
    this.errorMessage = '';
  }

  save(): void {
    if (!this.form.name || !this.form.category) {
      this.errorMessage = 'Name and Category are required.';
      return;
    }

    const obs = this.editMode && this.selectedProduct
      ? this.productService.update(this.selectedProduct.productID, this.form)
      : this.productService.create(this.form);

    obs.subscribe({
      next: () => {
        this.successMessage = `Product ${this.editMode ? 'updated' : 'created'} successfully.`;
        this.showForm = false;
        this.errorMessage = '';
        this.loadProducts();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = `Failed to ${this.editMode ? 'update' : 'create'} product.`;
      }
    });
  }

  setStatus(p: ProductViewModel, status: string): void {
    this.productService.updateStatus(p.productID, status).subscribe({
      next: () => {
        this.successMessage = `Product status set to ${status}.`;
        this.loadProducts();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to update product status.';
      }
    });
  }

  getStatusBadgeClass(status: string): string {
    switch (status) {
      case 'Active': return 'badge bg-success';
      case 'Discontinued': return 'badge bg-danger';
      default: return 'badge bg-secondary';
    }
  }

  cancel(): void {
    this.showForm = false;
    this.errorMessage = '';
  }
}
