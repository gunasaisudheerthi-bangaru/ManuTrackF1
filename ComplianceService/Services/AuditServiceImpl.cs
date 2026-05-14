using ComplianceService.DTOs;
using ComplianceService.Models;
using ComplianceService.DTOs;
using ComplianceService.Repositories.Interfaces;
using ComplianceService.Services.Interfaces;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;

namespace ComplianceService.Services;

public class AuditServiceImpl(IAuditRepository repo) : IAuditService
{
    public async Task<ApiResponse<IEnumerable<AuditEntryViewModel>>> GetAllAsync(
        string? userId, string? serviceName, DateTime? from, DateTime? to)
    {
        var entries = await repo.GetAllAsync(userId, serviceName, from, to);
        return ApiResponse<IEnumerable<AuditEntryViewModel>>.Ok(entries.Select(Map));
    }

    public async Task<ApiResponse<AuditEntryViewModel>> GetByIdAsync(int id)
    {
        var entry = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Audit entry {id} not found.");
        return ApiResponse<AuditEntryViewModel>.Ok(Map(entry));
    }

    public async Task<ApiResponse<AuditEntryViewModel>> LogAsync(LogAuditEntryRequest request)
    {
        var entry = new AuditEntry
        {
            UserID = request.UserID,
            UserName = request.UserName,
            Action = request.Action,
            EntityType = request.EntityType,
            EntityID = request.EntityID,
            ServiceName = request.ServiceName,
            Details = request.Details,
            Timestamp = DateTime.UtcNow
        };

        var created = await repo.CreateAsync(entry);
        return ApiResponse<AuditEntryViewModel>.Ok(Map(created), "Audit entry logged.");
    }

    private static AuditEntryViewModel Map(AuditEntry a) => new()
    {
        AuditID = a.AuditID,
        UserID = a.UserID,
        UserName = a.UserName,
        Action = a.Action,
        EntityType = a.EntityType,
        EntityID = a.EntityID,
        ServiceName = a.ServiceName,
        Details = a.Details,
        Timestamp = a.Timestamp
    };
}

