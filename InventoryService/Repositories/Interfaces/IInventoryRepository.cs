using InventoryService.Models;

namespace InventoryService.Repositories.Interfaces;

public interface IInventoryRepository
{
    Task<IEnumerable<InventoryItem>> GetAllAsync(string? status = null, int? locationId = null);
    Task<InventoryItem?> GetByIdAsync(int id);
    Task<InventoryItem?> GetByProductIdAsync(int productId);
    Task<IEnumerable<InventoryItem>> GetLowStockAsync();
    Task<InventoryItem> CreateAsync(InventoryItem item);
    Task<InventoryItem> UpdateAsync(InventoryItem item);
    Task DeleteAsync(InventoryItem item);
    Task<bool> ExistsAsync(int id);
}
