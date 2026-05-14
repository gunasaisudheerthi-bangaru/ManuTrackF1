using ComplianceService.DTOs;
using ComplianceService.DTOs;
using ManuTrack.SharedKernel.Responses;

namespace ComplianceService.Services.Interfaces;

public interface IAuditService
{
    Task<ApiResponse<IEnumerable<AuditEntryViewModel>>> GetAllAsync(string? userId, string? serviceName, DateTime? from, DateTime? to);
    Task<ApiResponse<AuditEntryViewModel>> GetByIdAsync(int id);
    Task<ApiResponse<AuditEntryViewModel>> LogAsync(LogAuditEntryRequest request);
}

