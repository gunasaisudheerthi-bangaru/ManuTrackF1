import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  InspectionViewModel, CreateInspectionRequest,
  DefectViewModel, CreateDefectRequest
} from '../models/quality.model';
import { ApiResponse } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class QualityService {
  private inspUrl = `${environment.apiUrl}/inspections`;
  private defUrl = `${environment.apiUrl}/defects`;

  constructor(private http: HttpClient) {}

  // ── Inspections ──────────────────────────────────────────────────────────
  getInspections(status?: string, workOrderId?: number): Observable<ApiResponse<InspectionViewModel[]>> {
    const params = new URLSearchParams();
    if (status) params.set('status', status);
    if (workOrderId) params.set('workOrderId', workOrderId.toString());
    const query = params.toString() ? `?${params}` : '';
    return this.http.get<ApiResponse<InspectionViewModel[]>>(`${this.inspUrl}${query}`);
  }

  createInspection(request: CreateInspectionRequest): Observable<ApiResponse<InspectionViewModel>> {
    return this.http.post<ApiResponse<InspectionViewModel>>(this.inspUrl, request);
  }

  submitResult(id: number, result: string, notes: string): Observable<ApiResponse<InspectionViewModel>> {
    return this.http.put<ApiResponse<InspectionViewModel>>(`${this.inspUrl}/${id}/result`, { result, notes });
  }

  // ── Defects ──────────────────────────────────────────────────────────────
  getDefects(status?: string, severity?: string): Observable<ApiResponse<DefectViewModel[]>> {
    const params = new URLSearchParams();
    if (status) params.set('status', status);
    if (severity) params.set('severity', severity);
    const query = params.toString() ? `?${params}` : '';
    return this.http.get<ApiResponse<DefectViewModel[]>>(`${this.defUrl}${query}`);
  }

  getDefectsByInspection(inspectionId: number): Observable<ApiResponse<DefectViewModel[]>> {
    return this.http.get<ApiResponse<DefectViewModel[]>>(`${this.defUrl}/inspection/${inspectionId}`);
  }

  createDefect(request: CreateDefectRequest): Observable<ApiResponse<DefectViewModel>> {
    return this.http.post<ApiResponse<DefectViewModel>>(this.defUrl, request);
  }

  resolveDefect(id: number, resolution: string): Observable<ApiResponse<DefectViewModel>> {
    return this.http.put<ApiResponse<DefectViewModel>>(`${this.defUrl}/${id}/resolve`, { resolution });
  }
}
