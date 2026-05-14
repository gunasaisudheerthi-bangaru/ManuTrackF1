using QualityService.Models;

namespace QualityService.Repositories.Interfaces;

public interface IDefectRepository
{
    Task<IEnumerable<Defect>> GetByInspectionIdAsync(int inspectionId);
    Task<IEnumerable<Defect>> GetAllAsync(string? status = null, string? severity = null);
    Task<Defect?> GetByIdAsync(int id);
    Task<Defect> CreateAsync(Defect defect);
    Task<Defect> UpdateAsync(Defect defect);
    Task<bool> ExistsAsync(int id);
}
