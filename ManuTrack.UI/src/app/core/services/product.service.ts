import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ProductViewModel {
  productID: number;
  name: string;
  category: string;
  version: string;
  status: string;
  description?: string;
  createdDate: string;
}

export interface CreateProductRequest {
  name: string;
  category: string;
  version: string;
  description?: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly base = 'http://localhost:5000/api/v1/products';

  constructor(private http: HttpClient) {}

  getAll(category?: string, status?: string): Observable<ApiResponse<ProductViewModel[]>> {
    let url = this.base;
    const params: string[] = [];
    if (category) params.push(`category=${category}`);
    if (status) params.push(`status=${status}`);
    if (params.length) url += '?' + params.join('&');
    return this.http.get<ApiResponse<ProductViewModel[]>>(url);
  }

  create(req: CreateProductRequest): Observable<ApiResponse<ProductViewModel>> {
    return this.http.post<ApiResponse<ProductViewModel>>(this.base, req);
  }

  updateStatus(id: number, status: string): Observable<ApiResponse<ProductViewModel>> {
    return this.http.put<ApiResponse<ProductViewModel>>(`${this.base}/${id}/status`, { status });
  }

  delete(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.base}/${id}`);
  }
}
