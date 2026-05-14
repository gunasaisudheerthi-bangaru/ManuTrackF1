using QualityService.Models;

namespace QualityService.Repositories.Interfaces;

public interface IInspectionRepository
{
    Task<IEnumerable<Inspection>> GetAllAsync(string? status = null, int? workOrderId = null);
    Task<Inspection?> GetByIdAsync(int id);
    Task<Inspection?> GetByIdWithDefectsAsync(int id);
    Task<Inspection> CreateAsync(Inspection inspection);
    Task<Inspection> UpdateAsync(Inspection inspection);
    Task<bool> ExistsAsync(int id);
}
