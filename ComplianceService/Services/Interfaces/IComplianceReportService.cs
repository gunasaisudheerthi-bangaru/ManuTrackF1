using ComplianceService.DTOs;
using ComplianceService.DTOs;
using ManuTrack.SharedKernel.Responses;

namespace ComplianceService.Services.Interfaces;

public interface IComplianceReportService
{
    Task<ApiResponse<IEnumerable<ComplianceReportViewModel>>> GetAllAsync(string? status, string? reportType);
    Task<ApiResponse<ComplianceReportViewModel>> GetByIdAsync(int id);
    Task<ApiResponse<ComplianceReportViewModel>> CreateAsync(CreateComplianceReportRequest request, string generatedBy);
    Task<ApiResponse<ComplianceReportViewModel>> UpdateStatusAsync(int id, UpdateReportStatusRequest request);
    Task<ApiResponse<ComplianceReportViewModel>> ApproveReportAsync(int id, ApproveReportRequest request);
}

