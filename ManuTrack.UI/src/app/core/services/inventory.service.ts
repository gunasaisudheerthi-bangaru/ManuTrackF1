import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from './product.service';

export interface InventoryItemViewModel {
  inventoryID: number;
  itemType: string;
  productID?: number;
  componentID?: number;
  productName: string;
  locationID?: number;
  locationName?: string;
  quantityOnHand: number;
  minimumQuantity: number;
  status: string;
  notes?: string;
  createdDate: string;
}

export interface PurchaseOrderViewModel {
  poid: number;
  supplierID: string;
  supplierName: string;
  orderDate: string;
  expectedDeliveryDate: string;
  status: string;
  totalAmount: number;
  notes?: string;
  items: PurchaseOrderItemViewModel[];
}

export interface PurchaseOrderItemViewModel {
  poItemID: number;
  inventoryID: number;
  productID: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface SupplierViewModel {
  supplierID: number;
  name: string;
  contactPerson?: string;
  phone?: string;
  email?: string;
  address?: string;
  isActive: boolean;
}

export interface CreateInventoryItemRequest {
  itemType: 'Product' | 'RawMaterial';
  productID?: number;
  componentID?: number;
  productName: string;
  quantityOnHand: number;
  minimumQuantity: number;
  notes?: string;
}

export interface AdjustQuantityRequest {
  adjustment: number;
  reason: string;
}

export interface CreatePurchaseOrderRequest {
  supplierName: string;
  expectedDeliveryDate: string;
  notes?: string;
  items: { inventoryID: number; productID: number; productName: string; quantity: number; unitPrice: number }[];
}

export interface CreateSupplierRequest {
  name: string;
  contactPerson?: string;
  phone?: string;
  email?: string;
  address?: string;
}

@Injectable({ providedIn: 'root' })
export class InventoryService {
  private readonly invBase  = '/api/v1/inventory';
  private readonly poBase   = '/api/v1/purchase-orders';
  private readonly supBase  = '/api/v1/suppliers';

  constructor(private http: HttpClient) {}

  // ── Inventory ──────────────────────────────────────────
  getAll(status?: string): Observable<ApiResponse<InventoryItemViewModel[]>> {
    const url = status ? `${this.invBase}?status=${status}` : this.invBase;
    return this.http.get<ApiResponse<InventoryItemViewModel[]>>(url);
  }
  getLowStock(): Observable<ApiResponse<InventoryItemViewModel[]>> {
    return this.http.get<ApiResponse<InventoryItemViewModel[]>>(`${this.invBase}/low-stock`);
  }
  create(req: CreateInventoryItemRequest): Observable<ApiResponse<InventoryItemViewModel>> {
    return this.http.post<ApiResponse<InventoryItemViewModel>>(this.invBase, req);
  }
  adjust(id: number, req: AdjustQuantityRequest): Observable<ApiResponse<InventoryItemViewModel>> {
    return this.http.put<ApiResponse<InventoryItemViewModel>>(`${this.invBase}/${id}/adjust`, req);
  }
  updateMinimumQty(id: number, minimumQuantity: number): Observable<ApiResponse<InventoryItemViewModel>> {
    return this.http.put<ApiResponse<InventoryItemViewModel>>(`${this.invBase}/${id}`, { minimumQuantity });
  }
  delete(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.invBase}/${id}`);
  }

  // ── Purchase Orders ────────────────────────────────────
  getAllPO(status?: string): Observable<ApiResponse<PurchaseOrderViewModel[]>> {
    const url = status ? `${this.poBase}?status=${status}` : this.poBase;
    return this.http.get<ApiResponse<PurchaseOrderViewModel[]>>(url);
  }
  createPO(req: CreatePurchaseOrderRequest): Observable<ApiResponse<PurchaseOrderViewModel>> {
    return this.http.post<ApiResponse<PurchaseOrderViewModel>>(this.poBase, req);
  }
  updatePOStatus(id: number, status: string): Observable<ApiResponse<PurchaseOrderViewModel>> {
    return this.http.put<ApiResponse<PurchaseOrderViewModel>>(`${this.poBase}/${id}/status`, { status });
  }

  // ── Suppliers ──────────────────────────────────────────
  getAllSuppliers(): Observable<ApiResponse<SupplierViewModel[]>> {
    return this.http.get<ApiResponse<SupplierViewModel[]>>(this.supBase);
  }
  createSupplier(req: CreateSupplierRequest): Observable<ApiResponse<SupplierViewModel>> {
    return this.http.post<ApiResponse<SupplierViewModel>>(this.supBase, req);
  }
}
