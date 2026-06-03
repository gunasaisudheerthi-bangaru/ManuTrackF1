import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from './product.service';

export interface ComponentViewModel {
  componentID: number;
  name: string;
  materialType: string;
  unit: string;
  description?: string;
  isActive: boolean;
  createdDate: string;
}

export interface CreateComponentRequest {
  name: string;
  materialType: string;
  unit: string;
  description?: string;
}

@Injectable({ providedIn: 'root' })
export class ComponentService {
  private readonly base = 'http://localhost:5000/api/v1/components';

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApiResponse<ComponentViewModel[]>> {
    return this.http.get<ApiResponse<ComponentViewModel[]>>(this.base);
  }

  create(req: CreateComponentRequest): Observable<ApiResponse<ComponentViewModel>> {
    return this.http.post<ApiResponse<ComponentViewModel>>(this.base, req);
  }

  delete(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.base}/${id}`);
  }
}
