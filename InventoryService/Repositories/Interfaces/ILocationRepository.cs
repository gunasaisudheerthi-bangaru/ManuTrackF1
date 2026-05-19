using InventoryService.Models;

namespace InventoryService.Repositories.Interfaces;

public interface ILocationRepository
{
    Task<IEnumerable<InventoryLocation>> GetAllAsync(bool? isActive = null);
    Task<InventoryLocation?> GetByIdAsync(int id);
    Task<InventoryLocation> CreateAsync(InventoryLocation location);
    Task<InventoryLocation> UpdateAsync(InventoryLocation location);
    Task DeleteAsync(InventoryLocation location);
    Task<bool> ExistsAsync(int id);
    Task<bool> HasItemsAsync(int locationId);
}
