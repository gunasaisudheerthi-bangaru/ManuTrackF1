import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from './product.service';

export interface InspectionViewModel {
  inspectionID: number;
  workOrderID: number;
  inspectionDate: string;
  inspectorID: string;
  inspectorName: string;
  result: string;
  status: string;
  notes?: string;
  createdDate: string;
  totalDefectCount: number;
  criticalCount: number;
  highCount: number;
  mediumCount: number;
  lowCount: number;
}

export interface DefectViewModel {
  defectID: number;
  inspectionID: number;
  description: string;
  severity: string;
  status: string;
  resolutionDescription?: string;
  createdDate: string;
  resolvedDate?: string;
}

export interface CreateInspectionRequest {
  workOrderID: number;
  inspectionDate: string;
  inspectorID: string;
  inspectorName: string;
  notes?: string;
}

export interface CreateDefectRequest {
  inspectionID: number;
  description: string;
  severity: string;
}

@Injectable({ providedIn: 'root' })
export class QualityService {
  private readonly inspBase = '/api/v1/inspections';
  private readonly defBase  = '/api/v1/defects';

  constructor(private http: HttpClient) {}

  // Inspections
  getAllInspections(status?: string): Observable<ApiResponse<InspectionViewModel[]>> {
    const url = status ? `${this.inspBase}?status=${status}` : this.inspBase;
    return this.http.get<ApiResponse<InspectionViewModel[]>>(url);
  }
  createInspection(req: CreateInspectionRequest): Observable<ApiResponse<InspectionViewModel>> {
    return this.http.post<ApiResponse<InspectionViewModel>>(this.inspBase, req);
  }
  updateInspectionResult(id: number, result: string, status: string, notes?: string): Observable<ApiResponse<InspectionViewModel>> {
    return this.http.put<ApiResponse<InspectionViewModel>>(`${this.inspBase}/${id}/result`, { result, status, notes });
  }

  // Defects
  getAllDefects(status?: string): Observable<ApiResponse<DefectViewModel[]>> {
    const url = status ? `${this.defBase}?status=${status}` : this.defBase;
    return this.http.get<ApiResponse<DefectViewModel[]>>(url);
  }
  getDefectsByInspection(inspectionId: number): Observable<ApiResponse<DefectViewModel[]>> {
    return this.http.get<ApiResponse<DefectViewModel[]>>(`${this.defBase}/inspection/${inspectionId}`);
  }
  createDefect(req: CreateDefectRequest): Observable<ApiResponse<DefectViewModel>> {
    return this.http.post<ApiResponse<DefectViewModel>>(this.defBase, req);
  }
  resolveDefect(id: number, resolutionDescription: string): Observable<ApiResponse<DefectViewModel>> {
    return this.http.put<ApiResponse<DefectViewModel>>(`${this.defBase}/${id}/resolve`, { resolutionDescription });
  }
  updateDefectStatus(id: number, status: string): Observable<ApiResponse<DefectViewModel>> {
    return this.http.put<ApiResponse<DefectViewModel>>(`${this.defBase}/${id}/status`, { status });
  }
}
