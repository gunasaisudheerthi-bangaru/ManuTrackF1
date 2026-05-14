import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ProductViewModel, CreateProductRequest, BomViewModel, CreateBomRequest } from '../models/product.model';
import { ApiResponse } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private url = `${environment.apiUrl}/products`;
  private bomUrl = `${environment.apiUrl}/bom`;

  constructor(private http: HttpClient) {}

  // ── Products ─────────────────────────────────────────────────────────────
  getAll(category?: string, status?: string): Observable<ApiResponse<ProductViewModel[]>> {
    const params = new URLSearchParams();
    if (category) params.set('category', category);
    if (status) params.set('status', status);
    const query = params.toString() ? `?${params}` : '';
    return this.http.get<ApiResponse<ProductViewModel[]>>(`${this.url}${query}`);
  }

  getById(id: number): Observable<ApiResponse<ProductViewModel>> {
    return this.http.get<ApiResponse<ProductViewModel>>(`${this.url}/${id}`);
  }

  create(request: CreateProductRequest): Observable<ApiResponse<ProductViewModel>> {
    return this.http.post<ApiResponse<ProductViewModel>>(this.url, request);
  }

  update(id: number, request: CreateProductRequest): Observable<ApiResponse<ProductViewModel>> {
    return this.http.put<ApiResponse<ProductViewModel>>(`${this.url}/${id}`, request);
  }

  updateStatus(id: number, status: string): Observable<ApiResponse<ProductViewModel>> {
    return this.http.put<ApiResponse<ProductViewModel>>(`${this.url}/${id}/status`, { status });
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.url}/${id}`);
  }

  // ── BOM ──────────────────────────────────────────────────────────────────
  getBom(productId: number): Observable<ApiResponse<BomViewModel[]>> {
    return this.http.get<ApiResponse<BomViewModel[]>>(`${this.bomUrl}/product/${productId}`);
  }

  createBom(request: CreateBomRequest): Observable<ApiResponse<BomViewModel>> {
    return this.http.post<ApiResponse<BomViewModel>>(this.bomUrl, request);
  }
}
