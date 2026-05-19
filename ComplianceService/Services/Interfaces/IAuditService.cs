using ComplianceService.DTOs;
using ManuTrack.SharedKernel.Responses;

namespace ComplianceService.Services.Interfaces;

public interface IAuditService
{
    // Change 5: extended filters
    Task<ApiResponse<IEnumerable<AuditEntryViewModel>>> GetAllAsync(
        string? userId,
        string? serviceName,
        DateTime? from,
        DateTime? to,
        string? entityType,
        string? action,
        string? entityId);

    Task<ApiResponse<AuditEntryViewModel>> GetByIdAsync(int id);
    Task<ApiResponse<AuditEntryViewModel>> LogAsync(LogAuditEntryRequest request);
}
