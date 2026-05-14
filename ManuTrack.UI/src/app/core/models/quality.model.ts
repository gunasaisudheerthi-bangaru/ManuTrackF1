// Matches InspectionViewModel.cs in QualityService
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
  defectCount: number;
}

export interface CreateInspectionRequest {
  workOrderID: number;
  notes?: string;
}

// Matches DefectViewModel.cs in QualityService
export interface DefectViewModel {
  defectID: number;
  inspectionID: number;
  description: string;
  severity: string;
  status: string;
  resolution?: string;
  createdDate: string;
  resolvedDate?: string;
}

export interface CreateDefectRequest {
  inspectionID: number;
  description: string;
  severity: string;
}
