using ManuTrack.SharedKernel.Responses;
using QualityService.DTOs;

namespace QualityService.Services.Interfaces;

public interface IDefectService
{
    Task<ApiResponse<IEnumerable<DefectViewModel>>> GetByInspectionIdAsync(int inspectionId);
    Task<ApiResponse<IEnumerable<DefectViewModel>>> GetAllAsync(string? status, string? severity);
    Task<ApiResponse<DefectViewModel>> GetByIdAsync(int id);
    Task<ApiResponse<DefectViewModel>> CreateAsync(CreateDefectRequest request);
    Task<ApiResponse<DefectViewModel>> ResolveAsync(int id, ResolveDefectRequest request);
    Task<ApiResponse<DefectViewModel>> UpdateStatusAsync(int id, UpdateDefectStatusRequest request);
}
