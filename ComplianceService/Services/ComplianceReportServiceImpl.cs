using ComplianceService.DTOs;
using ComplianceService.Models;
using ComplianceService.DTOs;
using ComplianceService.Repositories.Interfaces;
using ComplianceService.Services.Interfaces;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;

namespace ComplianceService.Services;

public class ComplianceReportServiceImpl(IComplianceReportRepository repo) : IComplianceReportService
{
    public async Task<ApiResponse<IEnumerable<ComplianceReportViewModel>>> GetAllAsync(string? status, string? reportType)
    {
        var reports = await repo.GetAllAsync(status, reportType);
        return ApiResponse<IEnumerable<ComplianceReportViewModel>>.Ok(reports.Select(Map));
    }

    public async Task<ApiResponse<ComplianceReportViewModel>> GetByIdAsync(int id)
    {
        var report = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Compliance report {id} not found.");
        return ApiResponse<ComplianceReportViewModel>.Ok(Map(report));
    }

    public async Task<ApiResponse<ComplianceReportViewModel>> CreateAsync(CreateComplianceReportRequest request, string generatedBy)
    {
        var report = new ComplianceReport
        {
            Title = request.Title,
            Scope = request.Scope,
            ReportType = request.ReportType,
            Metrics = request.Metrics,
            PeriodStart = request.PeriodStart,
            PeriodEnd = request.PeriodEnd,
            GeneratedBy = generatedBy,
            GeneratedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            Status = "Draft"
        };

        var created = await repo.CreateAsync(report);
        return ApiResponse<ComplianceReportViewModel>.Ok(Map(created), "Compliance report created.");
    }

    public async Task<ApiResponse<ComplianceReportViewModel>> UpdateStatusAsync(int id, UpdateReportStatusRequest request)
    {
        var report = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Compliance report {id} not found.");
        report.Status = request.Status;
        report.UpdatedDate = DateTime.UtcNow;
        var updated = await repo.UpdateAsync(report);
        return ApiResponse<ComplianceReportViewModel>.Ok(Map(updated), "Report status updated.");
    }

    public async Task<ApiResponse<ComplianceReportViewModel>> ApproveReportAsync(int id, ApproveReportRequest request)
    {
        var report = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Compliance report {id} not found.");
        if (report.Status != "InReview")
            throw new ValidationException("Report must be InReview status to approve.");
        report.ApprovedBy = request.ApprovedBy;
        report.ApprovedDate = DateTime.UtcNow;
        report.Status = "Approved";
        report.UpdatedDate = DateTime.UtcNow;
        var updated = await repo.UpdateAsync(report);
        return ApiResponse<ComplianceReportViewModel>.Ok(Map(updated), "Report approved.");
    }

    private static ComplianceReportViewModel Map(ComplianceReport r) => new()
    {
        ReportID = r.ReportID,
        Title = r.Title,
        Scope = r.Scope,
        Metrics = r.Metrics,
        GeneratedDate = r.GeneratedDate,
        GeneratedBy = r.GeneratedBy,
        Status = r.Status,
        ReportType = r.ReportType,
        PeriodStart = r.PeriodStart,
        PeriodEnd = r.PeriodEnd,
        CreatedDate = r.CreatedDate,
        UpdatedDate = r.UpdatedDate,
        ApprovedBy = r.ApprovedBy,
        ApprovedDate = r.ApprovedDate
    };
}

