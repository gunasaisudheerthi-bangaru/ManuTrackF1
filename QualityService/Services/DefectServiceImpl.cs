using QualityService.Enums;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;
using QualityService.DTOs;
using QualityService.Models;
using QualityService.Repositories.Interfaces;
using QualityService.Services.Interfaces;

namespace QualityService.Services;

public class DefectServiceImpl(IDefectRepository defectRepo, IInspectionRepository inspectionRepo) : IDefectService
{
    public async Task<ApiResponse<IEnumerable<DefectViewModel>>> GetByInspectionIdAsync(int inspectionId)
    {
        if (!await inspectionRepo.ExistsAsync(inspectionId))
            throw new NotFoundException($"Inspection {inspectionId} not found.");

        var defects = await defectRepo.GetByInspectionIdAsync(inspectionId);
        return ApiResponse<IEnumerable<DefectViewModel>>.Ok(defects.Select(Map));
    }

    public async Task<ApiResponse<IEnumerable<DefectViewModel>>> GetAllAsync(string? status, string? severity)
    {
        var defects = await defectRepo.GetAllAsync(status, severity);
        return ApiResponse<IEnumerable<DefectViewModel>>.Ok(defects.Select(Map));
    }

    public async Task<ApiResponse<DefectViewModel>> GetByIdAsync(int id)
    {
        var defect = await defectRepo.GetByIdAsync(id) ?? throw new NotFoundException($"Defect {id} not found.");
        return ApiResponse<DefectViewModel>.Ok(Map(defect));
    }

    public async Task<ApiResponse<DefectViewModel>> CreateAsync(CreateDefectRequest request)
    {
        if (!await inspectionRepo.ExistsAsync(request.InspectionID))
            throw new NotFoundException($"Inspection {request.InspectionID} not found.");

        var defect = new Defect
        {
            InspectionID = request.InspectionID,
            Description = request.Description,
            Severity = request.Severity,
            Status = DefectStatus.Open,
            CreatedDate = DateTime.UtcNow
        };

        var created = await defectRepo.CreateAsync(defect);
        return ApiResponse<DefectViewModel>.Ok(Map(created), "Defect logged.");
    }

    public async Task<ApiResponse<DefectViewModel>> ResolveAsync(int id, ResolveDefectRequest request)
    {
        var defect = await defectRepo.GetByIdAsync(id) ?? throw new NotFoundException($"Defect {id} not found.");
        defect.Resolution = request.Resolution;
        defect.Status = DefectStatus.Resolved;
        defect.ResolvedDate = DateTime.UtcNow;
        defect.UpdatedDate = DateTime.UtcNow;

        var updated = await defectRepo.UpdateAsync(defect);
        return ApiResponse<DefectViewModel>.Ok(Map(updated), "Defect resolved.");
    }

    public async Task<ApiResponse<DefectViewModel>> UpdateStatusAsync(int id, UpdateDefectStatusRequest request)
    {
        var defect = await defectRepo.GetByIdAsync(id) ?? throw new NotFoundException($"Defect {id} not found.");
        defect.Status = request.Status;
        defect.UpdatedDate = DateTime.UtcNow;

        var updated = await defectRepo.UpdateAsync(defect);
        return ApiResponse<DefectViewModel>.Ok(Map(updated), "Defect status updated.");
    }

    private static DefectViewModel Map(Defect d) => new()
    {
        DefectID = d.DefectID,
        InspectionID = d.InspectionID,
        Description = d.Description,
        Severity = d.Severity,
        Status = d.Status,
        Resolution = d.Resolution,
        CreatedDate = d.CreatedDate,
        UpdatedDate = d.UpdatedDate,
        ResolvedDate = d.ResolvedDate
    };
}
