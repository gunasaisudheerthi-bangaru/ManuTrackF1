// Matches ComplianceReportViewModel.cs in ComplianceService
export interface ComplianceReportViewModel {
  reportID: number;
  title: string;
  scope: string;
  metrics: string;
  generatedDate: string;
  generatedBy: string;
  status: string;
  reportType: string;
  periodStart?: string;
  periodEnd?: string;
}

export interface CreateComplianceReportRequest {
  reportType: string;
  scope: string;
  periodStart?: string;
  periodEnd?: string;
}

// Matches AuditEntryViewModel.cs in ComplianceService
export interface AuditLogViewModel {
  auditID: number;
  userID: string;
  userName: string;
  action: string;
  entityType: string;
  entityID: string;
  serviceName: string;
  details?: string;
  timestamp: string;
}
