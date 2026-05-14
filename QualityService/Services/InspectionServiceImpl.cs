using QualityService.Enums;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;
using QualityService.DTOs;
using QualityService.Models;
using QualityService.Repositories.Interfaces;
using QualityService.Services.Interfaces;

namespace QualityService.Services;

public class InspectionServiceImpl(IInspectionRepository repo) : IInspectionService
{
    public async Task<ApiResponse<IEnumerable<InspectionViewModel>>> GetAllAsync(string? status, int? workOrderId)
    {
        var items = await repo.GetAllAsync(status, workOrderId);
        return ApiResponse<IEnumerable<InspectionViewModel>>.Ok(items.Select(Map));
    }

    public async Task<ApiResponse<InspectionViewModel>> GetByIdAsync(int id)
    {
        var inspection = await repo.GetByIdWithDefectsAsync(id)
            ?? throw new NotFoundException($"Inspection {id} not found.");
        return ApiResponse<InspectionViewModel>.Ok(Map(inspection));
    }

    public async Task<ApiResponse<InspectionViewModel>> CreateAsync(CreateInspectionRequest request)
    {
        var inspection = new Inspection
        {
            WorkOrderID = request.WorkOrderID,
            InspectionDate = request.InspectionDate,
            InspectorID = request.InspectorID,
            InspectorName = request.InspectorName,
            Notes = request.Notes,
            Result = string.Empty,
            Status = InspectionStatus.Scheduled,
            CreatedDate = DateTime.UtcNow
        };

        var created = await repo.CreateAsync(inspection);
        return ApiResponse<InspectionViewModel>.Ok(Map(created), "Inspection created.");
    }

    public async Task<ApiResponse<InspectionViewModel>> UpdateResultAsync(int id, UpdateInspectionResultRequest request)
    {
        var inspection = await repo.GetByIdWithDefectsAsync(id)
            ?? throw new NotFoundException($"Inspection {id} not found.");

        inspection.Result = request.Result;
        inspection.Status = request.Status;
        if (request.Notes != null) inspection.Notes = request.Notes;
        inspection.UpdatedDate = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(inspection);
        return ApiResponse<InspectionViewModel>.Ok(Map(updated), "Inspection result updated.");
    }

    private static InspectionViewModel Map(Inspection i) => new()
    {
        InspectionID = i.InspectionID,
        WorkOrderID = i.WorkOrderID,
        InspectionDate = i.InspectionDate,
        InspectorID = i.InspectorID,
        InspectorName = i.InspectorName,
        Result = i.Result,
        Status = i.Status,
        Notes = i.Notes,
        CreatedDate = i.CreatedDate,
        UpdatedDate = i.UpdatedDate,
        DefectCount = i.Defects.Count
    };
}
