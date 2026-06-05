import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from './product.service';

export interface BomViewModel {
  bomID: number;
  productID: number;
  productName: string;
  componentID: number;
  componentName: string;
  componentUnit: string;
  componentMaterialType: string;
  quantity: number;
  version: string;
  status: string;
  notes?: string;
  createdDate: string;
}

export interface CreateBomRequest {
  productID: number;
  componentID: number;
  quantity: number;
  version: string;
  notes?: string;
}

@Injectable({ providedIn: 'root' })
export class BomService {
  private readonly base = '/api/v1/bom';

  constructor(private http: HttpClient) {}

  getAll(productId?: number): Observable<ApiResponse<BomViewModel[]>> {
    const url = productId ? `${this.base}?productId=${productId}` : this.base;
    return this.http.get<ApiResponse<BomViewModel[]>>(url);
  }

  getByProduct(productId: number): Observable<ApiResponse<BomViewModel[]>> {
    return this.http.get<ApiResponse<BomViewModel[]>>(`${this.base}/product/${productId}`);
  }

  create(req: CreateBomRequest): Observable<ApiResponse<BomViewModel>> {
    return this.http.post<ApiResponse<BomViewModel>>(this.base, req);
  }

  delete(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.base}/${id}`);
  }
}
