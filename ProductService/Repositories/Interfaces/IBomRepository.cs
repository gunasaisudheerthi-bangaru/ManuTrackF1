using ProductService.Models;

namespace ProductService.Repositories.Interfaces;

public interface IBomRepository
{
    Task<IEnumerable<Bom>> GetAllAsync(int? productId = null, string? status = null);
    Task<Bom?> GetByIdAsync(int id);
    Task<IEnumerable<Bom>> GetByProductIdAsync(int productId);
    Task<Bom> CreateAsync(Bom bom);
    Task<Bom> UpdateAsync(Bom bom);
    Task DeleteAsync(Bom bom);
    Task<bool> ExistsAsync(int id);
}
