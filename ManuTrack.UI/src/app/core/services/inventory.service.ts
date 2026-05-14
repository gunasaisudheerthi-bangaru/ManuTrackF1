import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  InventoryViewModel, AdjustQuantityRequest,
  PurchaseOrderViewModel, CreatePurchaseOrderRequest
} from '../models/inventory.model';
import { ApiResponse } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class InventoryService {
  private url = `${environment.apiUrl}/inventory`;
  private poUrl = `${environment.apiUrl}/purchase-orders`;

  constructor(private http: HttpClient) {}

  // ── Inventory ────────────────────────────────────────────────────────────
  getAll(status?: string): Observable<ApiResponse<InventoryViewModel[]>> {
    const params = status ? `?status=${status}` : '';
    return this.http.get<ApiResponse<InventoryViewModel[]>>(`${this.url}${params}`);
  }

  getById(id: number): Observable<ApiResponse<InventoryViewModel>> {
    return this.http.get<ApiResponse<InventoryViewModel>>(`${this.url}/${id}`);
  }

  getLowStock(): Observable<ApiResponse<InventoryViewModel[]>> {
    return this.http.get<ApiResponse<InventoryViewModel[]>>(`${this.url}/low-stock`);
  }

  adjustQuantity(inventoryID: number, quantity: number, reason?: string): Observable<ApiResponse<InventoryViewModel>> {
    return this.http.put<ApiResponse<InventoryViewModel>>(
      `${this.url}/${inventoryID}/adjust`, { quantity, reason }
    );
  }

  // ── Purchase Orders ──────────────────────────────────────────────────────
  getPurchaseOrders(status?: string): Observable<ApiResponse<PurchaseOrderViewModel[]>> {
    const params = status ? `?status=${status}` : '';
    return this.http.get<ApiResponse<PurchaseOrderViewModel[]>>(`${this.poUrl}${params}`);
  }

  createPurchaseOrder(request: CreatePurchaseOrderRequest): Observable<ApiResponse<PurchaseOrderViewModel>> {
    return this.http.post<ApiResponse<PurchaseOrderViewModel>>(this.poUrl, request);
  }

  updatePurchaseOrderStatus(id: number, status: string): Observable<ApiResponse<PurchaseOrderViewModel>> {
    return this.http.put<ApiResponse<PurchaseOrderViewModel>>(`${this.poUrl}/${id}/status`, { status });
  }
}
